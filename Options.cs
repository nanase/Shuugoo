using System.IO;
using Newtonsoft.Json;

namespace Shuugoo
{
    public class Options
    {
        #region -- Public Properties --

        public string ConsumerKey { get; set; }

        public string ConsumerSecret { get; set; }

        public string AccessToken { get; set; }

        public string AccessSecret { get; set; }

        public string BaseString { get; set; } = "集合";

        public string AdditionalString { get; set; } = "！";

        public int MaximumAdditionNumber { get; set; } = 10;

        public int CurrentAdditionNumber { get; set; }

        public bool NoConfirmingDialog { get; set; }

        public bool TopMost { get; set; }

        #endregion

        #region -- Public Methods --

        public void Save(string path)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public static Options Load(string path)
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Options>(json);
        }

        /*public bool Validate()
        {
            return !string.IsNullOrWhiteSpace(this.ConsumerKey) && !string.IsNullOrWhiteSpace(this.ConsumerSecret) &&
                   !string.IsNullOrWhiteSpace(this.AccessToken) && !string.IsNullOrWhiteSpace(this.AccessSecret) &&
                   !string.IsNullOrWhiteSpace(this.BaseString) && this.BaseString.Length < 140 &&
                   this.AdditionalString != null && this.AdditionalString.Length < 140 &&
                   this.MaximumAdditionNumber >= 0 && this.MaximumAdditionNumber <= 100;
        }*/

        #endregion
    }
}
