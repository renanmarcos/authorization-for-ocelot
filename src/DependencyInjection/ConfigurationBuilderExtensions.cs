using AuthorizationForOcelot.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

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
            List<string> excludedDirectories = new List<string>
            {
                "\\BIN",
                "\\OBJ"
            };            

            return new DirectoryInfo(environment.ContentRootPath)
                    .EnumerateFiles("*.json", SearchOption.AllDirectories)
                    .Where(
                        file => file.Name.Contains(options.OcelotConfigFileName) && 
                        !excludedDirectories.Any(directory => file.Directory.FullName.ToUpper().Contains(directory)));
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
