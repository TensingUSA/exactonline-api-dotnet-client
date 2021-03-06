﻿using ExactOnline.Client.Models.CRM;
using ExactOnline.Client.Sdk.Controllers;
using ExactOnline.Client.Sdk.Helpers;
using System;
using System.Diagnostics;
using System.Linq;

namespace ConsoleApplication
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			// To make this work set the authorisation properties of your test app in the testapp.config.
			var testApp = new TestApp();

			var connector = new Connector(testApp.ClientId.ToString(), testApp.ClientSecret, testApp.CallbackUrl);
			var client = new ExactOnlineClient(connector.EndPoint, connector.GetAccessToken);

			// Get the Code and Name of a random account in the administration.
			var fields = new[] { "Code", "Name" };
			var account = client.For<Account>().Top(1).Select(fields).Get().FirstOrDefault();
			Debug.WriteLine("Account {0} - {1}", account.Code.TrimStart(), account.Name);

			//This is an example of how to use skipToken for paging.
			string skipToken = string.Empty;
			var accounts = client.For<Account>().Select(fields).Get(ref skipToken);
			Debug.WriteLine("skipToken {0}", skipToken);

			//Now I can use the skip token to get the first record from the next page.
			var nextAccount = client.For<Account>().Top(1).Select(fields).Get(ref skipToken).FirstOrDefault();
			Debug.WriteLine("Account {0} - {1}", nextAccount.Code.TrimStart(), nextAccount.Name);
		}
	}
}
