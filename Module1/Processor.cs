namespace Module1
{
    using RestWrapper;
    using System;

    public class Processor
    {
        public Processor()
        {

        }

        public void Process(string url)
        {
            using (RestRequest req = new RestRequest(url))
            {
                using (RestResponse resp = req.Send())
                {
                    Console.WriteLine("| Status (module1): " + resp.StatusCode + " " + resp.ContentLength + " bytes");
                }
            }
        }
    }
}
