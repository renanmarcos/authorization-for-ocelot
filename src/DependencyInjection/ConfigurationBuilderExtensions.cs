using AuthorizationForOcelot.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AuthorizationForOcelot.DependencyInjection
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddOcelotWithAuthorization(
            this IConfigurationBuilder builder, 
            IHostEnvironment environment, 
            Action<OcelotWithAuthorizationOptions> action = null)
        {
            var options = new OcelotWithAuthorizationOptions();
            action?.Invoke(options);

            IEnumerable<FileInfo> ocelotFiles = GetOcelotFiles(environment, options);
            AuthorizationFileConfiguration fileConfiguration = MergeFilesOfOcelotConfiguration(ocelotFiles);

            byte[] buffer = JsonSerializer.SerializeToUtf8Bytes(fileConfiguration);
            MemoryStream stream = new MemoryStream(buffer);
            builder.AddJsonStream(stream);

            return builder;
        }

        private static IEnumerable<FileInfo> GetOcelotFiles(IHostEnvironment environment, OcelotWithAuthorizationOptions options)
        {
            var ocelotRegex = new Regex($"(?:{options.OcelotConfigFileName}\\.json)" +
                $"|(?:{options.OcelotConfigFileName}\\.{environment.EnvironmentName}\\.json)" +
                $"|(?:{options.OcelotConfigFileName}\\..*(?=\\.)\\.{environment.EnvironmentName}\\.json)", 
                RegexOptions.IgnoreCase);

            return new DirectoryInfo($"{environment.ContentRootPath}\\{options.OcelotFilesFolder}")
                    .EnumerateFiles("*.json")
                    .Where(file => ocelotRegex.IsMatch(file.Name));
        }

        private static AuthorizationFileConfiguration MergeFilesOfOcelotConfiguration(IEnumerable<FileInfo> files)
        {
            AuthorizationFileConfiguration fileConfigurationMerged = new AuthorizationFileConfiguration();

            foreach (FileInfo file in files)
            {
                string linesOfFile = File.ReadAllText(file.FullName);
                AuthorizationFileConfiguration config = JsonSerializer.Deserialize<AuthorizationFileConfiguration>(linesOfFile);

                fileConfigurationMerged.Routes.AddRange(config.Routes);
            }

            return fileConfigurationMerged;
        }
    }
}
