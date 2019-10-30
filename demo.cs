static void Main(string[] args)
        {
            Console.WriteLine("Detect faces:");
            Console.Write("Enter the path to an image with faces that you wish to analyze");
            string imageFilePath = Console.ReadLine();
            Console.WriteLine(imageFilePath);
            if (File.Exists(imageFilePath))
            {
                try
                {
                    MakeAnalysisRequestAsync(imageFilePath);
                    Console.WriteLine("\nWait a moment for the results to appear.\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n" + e.Message + "\nPress Enter to exit...\n");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid file path.\nPress Enter to exit...\n");
            }
            Console.ReadLine();
        }

        private static async System.Threading.Tasks.Task MakeAnalysisRequestAsync(string imageFilePath)
        {
            HttpClient client = new HttpClient();
            
            //request headers 
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // request parameters. 
            string requestParameters1 = "returnFaceId=true&returnFaceLandmarks=false" +
                "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses," +
                "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            string requestParameters = "faceIds=2";
                
            string uri = uriBase + apiDetect + "?" + requestParameters1;
            HttpResponseMessage response;
            byte[] byteData = GetImageAsByteArray(imageFilePath);
            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                //execute the REST API call
                response = await client.PostAsync(uri, content);
                string contentString = await response.Content.ReadAsStringAsync();

                //Display JSON response
                Console.WriteLine("\nResponse:");
                Console.WriteLine(JsonPrettyPrint(contentString));
                Console.WriteLine("");
            }

        }