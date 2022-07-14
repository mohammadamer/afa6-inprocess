using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;
using PnP.Core.Auth.Services.Builder.Configuration;
using afa_net_core_3._1;

[assembly: FunctionsStartup(typeof(OutdatedPages.Startup))]

namespace OutdatedPages
{
    public class Startup : FunctionsStartup
    {
        private ApplicationSettings ApplicationSettings;
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = builder.GetContext().Configuration;
            ApplicationSettings = new ApplicationSettings();
            config.Bind(ApplicationSettings);

            //builder.Services.AddPnPCore(options =>
            //{
            //    var authProvider = new X509CertificateAuthenticationProvider(ApplicationSettings.SharePointOnlineClientId,
            //        ApplicationSettings.TenantId,
            //        StoreName.My,
            //        StoreLocation.CurrentUser,
            //        ApplicationSettings.SharePointOnlineCertificateThumbPrint);
            //    options.DefaultAuthenticationProvider = authProvider;

            //});
            builder.Services.AddPnPCore(options =>
            {
                options.PnPContext.GraphFirst = false;
            });
            builder.Services.AddPnPCoreAuthentication(options =>
            {
                // Load the certificate to use
                X509Certificate2 cert = Utilities.LoadCertificate(ApplicationSettings);

                // Configure certificate based auth
                options.Credentials.Configurations.Add("CertAuth", new PnPCoreAuthenticationCredentialConfigurationOptions
                {
                    ClientId = ApplicationSettings.ClientId,
                    TenantId = ApplicationSettings.TenantId,
                    X509Certificate = new PnPCoreAuthenticationX509CertificateOptions
                    {
                        Certificate = cert,
                    }
                });
                options.Credentials.DefaultConfiguration = "CertAuth";
            });
            builder.Services.AddSingleton(sp =>
            {
                return ApplicationSettings;
            });

        }
    }
}
