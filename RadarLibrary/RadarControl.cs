using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RadarLibrary
{
    public class RadarControl
    {
        private readonly string _radarUri;
        private readonly HttpClient _httpClient;

        public bool IsRadarOnline { get; private set; }
        public List<Target> CurrentTargets { get; private set; }
        public string LastError { get; private set; }

        public RadarControl(string radarUri)
        {
            _radarUri = radarUri;
            _httpClient = new HttpClient();
            CurrentTargets = new List<Target>();
        }

        public async Task FetchRadarDataAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_radarUri}/scan_radars");
                if (!response.IsSuccessStatusCode)
                {
                    IsRadarOnline = false;
                    LastError = $"HTTP Error: {response.StatusCode}";
                    return;
                }

                string json = await response.Content.ReadAsStringAsync();
                ParseRadarData(json);
                IsRadarOnline = true;
            }
            catch (Exception ex)
            {
                IsRadarOnline = false;
                LastError = $"Exception: {ex.Message}";
            }
        }

        private void ParseRadarData(string json)
        {
            try
            {
                var data = JObject.Parse(json);
                var targets = data["targets"];

                if (targets != null)
                {
                    CurrentTargets.Clear();
                    foreach (var target in targets)
                    {
                        CurrentTargets.Add(new Target(
                            target["id"].Value<int>(),
                            target["distance"].Value<double>(),
                            target["angle"].Value<double>(),
                            target["speed"].Value<double>()
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                LastError = $"Parsing Error: {ex.Message}";
            }
        }
    }
}
