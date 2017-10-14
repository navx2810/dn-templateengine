using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.NodeServices;

namespace Mojo.Framework.Templating
{
    public interface IEngine
    {
        ITemplate Compile(string path);
    }

    public interface ITemplate
    {
        string Render(object vm);
    }

    public class Engine : IEngine
    {
        private INodeServices services;
        private IHostingEnvironment env;
        private Dictionary<string, string> handlers = new Dictionary<string, string>
        {
            { ".hbs", "hbs.compiler.js" }
        };
        public Engine(INodeServices serv, IHostingEnvironment env) { this.services = serv; this.env = env; }
        ITemplate IEngine.Compile(string relPath)
        {
            string fullPath = Path.Combine(env.WebRootPath, relPath);
            if (!File.Exists(fullPath)) { throw new FileNotFoundException($"The template path of {fullPath} does not exist."); }
            else
            {
                Task<string> Runner = Task.Run(() => ReadTemplate(fullPath));
                return new Template { Node = services, JSCompilerPath = Path.Combine(env.ContentRootPath, "node", (handlers[Path.GetExtension(fullPath)])), TemplateReadTask = Runner };
            }
        }
        private async Task<string> ReadTemplate(string path)
        {
            return await File.ReadAllTextAsync(path);
        }
    }

    public class Template : ITemplate
    {
        public Task<string> TemplateReadTask { get; set; }
        public INodeServices Node { get; set; }
        public string JSCompilerPath { get; set; }
        string ITemplate.Render(object vm)
        {
            TemplateReadTask.Wait();
            var res = Node.InvokeAsync<string>(JSCompilerPath, TemplateReadTask.Result, vm);
            res.Wait();
            return res.Result;
        }
    }

}