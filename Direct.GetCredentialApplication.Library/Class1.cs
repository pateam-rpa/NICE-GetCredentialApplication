using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Direct.Shared;
using Direct.Shared.Library;
using Direct.RTSMicroServicesSupport;
using Studio.Configuration;
using Direct.Interface;
using System.Configuration;
using Studio.Dom;
using log4net;


namespace Direct.GetCredentialApplication.Library
{

	[DirectDom("CCM")]
	public class CCM
	{
		private static readonly ILog logArchitect = LogManager.GetLogger(Loggers.LibraryObjects);
		[DirectDom("Get Application Id")]
		[DirectDomMethod("Get Application Id for {Application}")]
		[MethodDescription("Get Id for Credential Application")]
		public static int GetApplications(string applicationname)
		{
			try
			{
				CredentialManager manager = new CredentialManager();
				//DirectCollection<KeyValuePair> keyValuePairs = new DirectCollection<KeyValuePair>();
				IUserInfoServices userInfoServices = (IUserInfoServices)((IDirectFramework)AppDomain.CurrentDomain.GetData("Framework"))["UserInfo"];
			
				if (!userInfoServices.IsLoggedToServer)
				{
					if (logArchitect.IsDebugEnabled) { logArchitect.Debug("Direct.GetCredentialApplication.Library - User not connected to Server "); }
					return 0; 
				}


				Dictionary<int, ApplicationInfo> mapApplications = manager.GetApplicationsList(ServerConfiguration.ConfigManager.GetCurrentEnvironment().CCMConfiguration.URL);
				if (logArchitect.IsDebugEnabled) { logArchitect.Debug("Direct.GetCredentialApplication.Library - Looking for Application name"); }
				foreach (var item in mapApplications)
				{
					if (logArchitect.IsDebugEnabled) { logArchitect.Debug("Direct.GetCredentialApplication.Library - Adding Application: " + item.Value.name + " - " + item.Value.id); }
					if (item.Value.name == applicationname)
					{
						return item.Value.id;
					}
					//KeyValuePair _keyValue = new KeyValuePair
					//{
					//	Key = item.Value.id.ToString(),
					//	Value = item.Value.name
					//};
				
					//keyValuePairs.Add(_keyValue);
				}
					//return keyValuePairs;
					if (logArchitect.IsDebugEnabled) { logArchitect.Debug("Direct.GetCredentialApplication.Library - Application: " + applicationname + " not found"); }
					return 0;
			}
			catch (Exception e)
			{
				logArchitect.Error("Direct.GetCredentialApplication.Library - Get Credential Applications Exception", e);
				return 0;
			}
		}
	}
}
