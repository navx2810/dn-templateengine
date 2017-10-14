using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.NodeServices;

namespace Mojo.Framework
{
    public interface ITemplateEngine
    {
        Task<string> Render(string path, object VM);
    }

    public class TemplateEngine : ITemplateEngine
    {
        public TemplateEngine(INodeServices service, IHostingEnvironment env) { this.services = service; this.env = env; }
        private INodeServices services;
        private IHostingEnvironment env;

        private Dictionary<string, string> handlers = new Dictionary<string, string>
        {
            { ".hbs", "hbs.compiler.js" }
        };

        async Task<string> ITemplateEngine.Render(string path, object VM)
        {
            {
                string template;
                string res = string.Empty;
                path = Path.Combine(env.WebRootPath, path);
                if (!File.Exists(path)) { throw new FileNotFoundException($"The template path of {path} does not exist."); }
                else { template = File.ReadAllText(path); }

                string js_file;
                if (handlers.TryGetValue(Path.GetExtension(path), out js_file))
                {
                    res = await services.InvokeAsync<string>(Path.Combine(env.ContentRootPath, "node", js_file), template, VM);
                }
                return res;
            }
        }
    }
}