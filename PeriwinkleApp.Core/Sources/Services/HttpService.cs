using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PeriwinkleApp.Core.Sources.CommonInterfaces;
using PeriwinkleApp.Core.Sources.Exceptions;
using PeriwinkleApp.Core.Sources.Extensions;
using PeriwinkleApp.Core.Sources.Utils;

namespace PeriwinkleApp.Core.Sources.Services
{
    public class HttpService
    {
        public static string Tag => "HttpService";
        
    #region Properties
        
        private static HttpClient httpClient;
        public static  HttpClient GetHttpClient => httpClient ?? (httpClient = new HttpClient());

    #endregion

    #region Static Constructor

        static HttpService ()
        {
            if (httpClient == null)
                httpClient = new HttpClient ();
        }
        
    #endregion

    #region Public Methods

        // throws RequestFailedException
        public async Task <TResponse> PostReadResponse<TResponse, TPost> (string url, TPost obj) 
            where TResponse: class 
            where TPost: class
        {
            // convert object to json
            string json = JsonConvert.SerializeObject (obj);
            StringContent content = new StringContent (json);

            // default values
            string responseContent = null;
            TResponse tResponse = default (TResponse);

            try
            {
                // post the content and await from httpclient
                HttpClient client = HttpService.GetHttpClient;
                var response = await client.PostAsync (url, content);

                // throw failed request
                if (!response.IsSuccessStatusCode)
                    throw new RequestFailedException ($"Failed Post Request to {url}");

                // have a successful response, read the response content async
                responseContent = await response.Content.ReadAsStringAsync ();
                
                Logger.Log (responseContent, Tag);
            }
            catch (Exception e)
            {
                Logger.Log (e.Message, Tag);
                throw;
            }
            finally
            {
                // deserialize natin ung response na object
                if (responseContent != null)
                    tResponse = responseContent.JsonDeserialize <TResponse> (); //JsonUtils.JsonDeserialize <TResponse> (responseContent);
            }

            return tResponse;
        }



		public async Task <TResponse> PostByteArrayContent <TResponse> (string url, byte[] bytesContent)
		{
			// create the content
            ByteArrayContent byteArrayContent = new ByteArrayContent(bytesContent);

			// default values
			string responseContent = null;
			TResponse tResponse = default(TResponse);

			try
			{
				// post the content and await from httpclient
				HttpClient client = HttpService.GetHttpClient;
				var response = await client.PostAsync(url, byteArrayContent);

				// throw failed request
				if (!response.IsSuccessStatusCode)
					throw new RequestFailedException($"Failed Post Request to {url}");

				// have a successful response, read the response content async
				responseContent = await response.Content.ReadAsStringAsync();

				Logger.Log(responseContent, Tag);
			}
			catch (Exception e)
			{
				Logger.Log(e.Message, Tag);
				throw;
			}
			//			finally
			//			{
			//				// deserialize natin ung response na object
			//				if (responseContent != null)
			//					tResponse = responseContent.JsonDeserialize<TResponse>();
			//			}
			return tResponse;
        }

        public async Task <TResponse> PostMultipartFormDataContent <TResponse, TPost> (string url, TPost obj, byte[] bytesContent, string filename)
			where TResponse : class
			where TPost : class
        {
			Logger.Log (url);

			// create the content
			// TODO obj to json
			MultipartFormDataContent multipartContent = new MultipartFormDataContent ();

			// json object
			string json = JsonConvert.SerializeObject(obj);
			StringContent stringContent = new StringContent(json);
			stringContent.Headers.Add ("Content-Disposition", "form-data; name=\"json\"");
			multipartContent.Add(stringContent, "json");

			if (bytesContent != null && bytesContent.Length > 0)
			{
				var streamContent = new StreamContent(new MemoryStream(bytesContent));
				streamContent.Headers.Add("Content-Type", "application/octet-stream");
				multipartContent.Add(streamContent, "file", filename);
            }

            // default values
            string responseContent = null;
            TResponse tResponse = default(TResponse);

			try
			{
				// post the content and await from httpclient
				HttpClient client = HttpService.GetHttpClient;
				var response = await client.PostAsync(url, multipartContent);

				// throw failed request
				if (!response.IsSuccessStatusCode)
					throw new RequestFailedException($"Failed Post Request to {url}");

				// have a successful response, read the response content async
				responseContent = await response.Content.ReadAsStringAsync();

				Logger.Log(responseContent, Tag);
            }
			catch (Exception e)
			{
				Logger.Log(e.Message, Tag);
				throw;
            }
			finally
			{
				// deserialize natin ung response na object
				if (responseContent != null)
					tResponse = responseContent.JsonDeserialize<TResponse>();
			}
            return tResponse;
        }

        public async Task <TGet> GetWithParams <TGet> (string url, IEnumerable<KeyValuePair <string, string>> keyValuePairs)
            where TGet: class 
        {
            // generate the url with the needed GET Parameters
            string finalUri = url + "?" + keyValuePairs.ToGetParams ();

            // default values
            string responseContent = null;
            TGet tGet = default (TGet);

            try
            {
                // await the get response
                HttpClient client = HttpService.GetHttpClient;
                HttpResponseMessage response = await client.GetAsync (finalUri);

                // throw failed request
                if (!response.IsSuccessStatusCode)
                    throw new RequestFailedException ($"Failed GetWithParams Request to {finalUri}");

                // have a successful response, read the response content async
                responseContent = await response.Content.ReadAsStringAsync ();
            }
            catch (Exception e)
            {
                Logger.Log (e.Message, Tag);
                throw;
            }
            finally
            {
                // deserialize natin ung response na object
                if (responseContent != null)
                    tGet = responseContent.JsonDeserialize <TGet> ();
            }

            return tGet;
        }
        
        public async Task <TGet> GetAll <TGet> (string url)
            where TGet: class 
        {
            // default values
            string responseContent = null;
            TGet tGet = default (TGet);

            try
            {
                // await the get response
                HttpClient client = HttpService.GetHttpClient;
                HttpResponseMessage response = await client.GetAsync (url);

                // throw failed request
                if (!response.IsSuccessStatusCode)
                    throw new RequestFailedException ($"Failed GetWithParams Request to {url}");

                // have a successful response, read the response content async
                responseContent = await response.Content.ReadAsStringAsync ();
                Logger.Log (responseContent);
            }
            catch (Exception e)
            {
                Logger.Log (e.Message, Tag);
                throw;
            }
            finally
            {
                // deserialize natin ung response na object
                if (responseContent != null)
                    tGet = responseContent.JsonDeserialize <TGet> ();
            }

            return tGet;
        }

    #endregion

    }
}
