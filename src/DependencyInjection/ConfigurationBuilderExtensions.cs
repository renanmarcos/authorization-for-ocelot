using AuthorizationForOcelot.Configuration;
using Microsoft.Extensions.Configuration;
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
        public static IConfigurationBuilder AddOcelotWithAuthorization(this IConfigurationBuilder builder, Action<OcelotWithAuthorizationOptions> action = null)
        {
            var options = new OcelotWithAuthorizationOptions();
            action?.Invoke(options);

            AuthorizationFileConfiguration fileConfiguration = MergeFilesOfOcelotConfiguration(GetOcelotFiles(options));

            string path = $"{options.OcelotConfigFileName}.json";

            File.WriteAllText(path, JsonSerializer.Serialize(fileConfiguration));
            builder.AddJsonFile(path, optional: false, reloadOnChange: false);

            return builder;
        }

        private static IEnumerable<FileInfo> GetOcelotFiles(OcelotWithAuthorizationOptions options)
        {
            Regex ocelotPattern = new Regex($"^{options.OcelotConfigFileName}\\.(.*?)\\.json$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            
            IEnumerable<FileInfo> ocelotFiles = new DirectoryInfo(Directory.GetCurrentDirectory())
                    .EnumerateFiles("*", SearchOption.AllDirectories)
                    .Where(file => ocelotPattern.IsMatch(file.Name));

            string environment = options.HostEnvironment?.EnvironmentName;

            if (!string.IsNullOrWhiteSpace(environment))
            {
                ocelotFiles = ocelotFiles.Where(file => file.Name.Contains(environment));
            }

            return ocelotFiles;
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
