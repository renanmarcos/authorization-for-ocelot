using AuthorizationForOcelot.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AuthorizationForOcelot.DependencyInjection
{
    public static class ConfigurationBuilderExtensions
    {
        private const string OcelotFilePattern = @"^ocelot\.(.*?)\.json$";

        public static IConfigurationBuilder AddOcelotWithAuthorization(
            this IConfigurationBuilder builder,
            Action<OcelotWithAuthorizationOptions> action = null)
        {
            var options = new OcelotWithAuthorizationOptions();

            action?.Invoke(options);

            //new DirectoryInfo();

            File.WriteAllText(options.OcelotConfigFileName, JsonSerializer.Serialize(""));

            builder.AddJsonFile(options.OcelotConfigFileName, optional: false, reloadOnChange: false);

            return builder;
        }

        //private static FileInfo GetOcelotFile(string nameEnvironment)
        //{
        //    var reg = new Regex(OcelotFilePattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        //    FileInfo ocelotFile =
        //        new FileInfo("")
        //            .EnumerateFiles()
        //            .Where(fi => reg.IsMatch(fi.Name));

        //    if (!nameEnvirotment.IsNullOrWhiteSpace())
        //    {
        //        ocelotFiles = ocelotFiles.Where(fi => fi.Name.Contains(nameEnvirotment));
        //    }
        //}
    }
}
