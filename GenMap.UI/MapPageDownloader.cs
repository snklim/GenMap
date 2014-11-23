using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GenMap.UI
{
    public class MapPageDownloader
    {
        private HashSet<string> _tasks;

        public HashSet<string> Tasks
        {
            get { return _tasks ?? (_tasks = new HashSet<string>()); }
        }

        public void Download(string mapPageId)
        {
            if (Tasks.Contains(mapPageId)) return;

            Tasks.Add(mapPageId);

            var request = WebRequest.CreateHttp(
                string.Format("http://download.maps.vlasenko.net/smtm100/{0}.jpg", mapPageId));

            request.GetResponseAsync().ContinueWith(task =>
            {
                var response = task.Result;

                var buffer = new byte[1024];

                string mapPageFile = string.Format("{0}\\maps\\{1}.jpg",
                    Directory.GetCurrentDirectory(),
                    mapPageId);

                using (var streamReader = response.GetResponseStream())
                using (var streamWriter = File.Create(mapPageFile))
                {
                    if (streamReader != null)
                    {
                        int length;
                        while ((length = streamReader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            streamWriter.Write(buffer, 0, length);
                        }
                    }
                }

                Tasks.Remove(mapPageId);
            });
        }
    }
}
