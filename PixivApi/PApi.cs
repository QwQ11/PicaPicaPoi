using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Media.Imaging;
using System.Web;
using System.Net;
using System.Text.RegularExpressions;

namespace PixivApi
{
    public class PApi
    {
        const string LOGIN_URL = @"https://oauth.secure.pixiv.net/auth/token";
        const string ADD_ILLUST = @"https://app-api.pixiv.net/v1/illust/bookmark/add";
        const string DELETE_ILLUST = @"https://app-api.pixiv.net/v1/illust/bookmark/delete";
        const string RECOMMEDN_URL = @"https://app-api.pixiv.net/v1/illust/recommended?content_type=illust&filter=for_ios&include_ranking_label=true";


        public string AccessToken { get; internal set; }
        public string RefreshToken { get; internal set; }
        public BitmapImage Avatar { get
            {
                var wc = new WebClient();
                wc.Headers.Add(HttpRequestHeader.Referer, "http://spapi.pixiv.net/");
                var image = new BitmapImage();
                wc.DownloadDataAsync(new Uri(AvatarUrl));
                wc.DownloadDataCompleted += (s, e) =>
                {
                    var ms = new MemoryStream(e.Result);
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                };
                return image;
            }
        }
        internal string AvatarUrl { get; set; }
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string Account { get; internal set; }
        public bool IsPremium { get; internal set; }

        internal bool Login(string username, string password)
        {
            return Login(username, password, "");
        }
        internal bool Login(string refreshToken)
        {
            return Login("", "", refreshToken);
        }
        public Task<bool> LoginAsync(string username, string password)
        {
            return Task.Run(() => Login(username, password));
        }
        public Task<bool> LoginAsync(string refreshToken)
        {
            return Task.Run(() => Login(refreshToken));
        }
        internal bool Login(string username, string password,string refreshToken)
        {
            string postData;
            if (refreshToken == "")
            {
                postData = "client_id=" + "bYGKuGVw91e0NMfPGp44euvGt59s" + "&" +
                "client_secret=" + "HP3RmkgAmEGro0gn1x9ioawQE8WMfvLXDz3ZqxpK" + "&" +
                "grant_type=password" + "&" +
                "username=" + username + "&" +
                "password=" + password;
            } else
            {
                postData = "client_id=" + "bYGKuGVw91e0NMfPGp44euvGt59s" + "&" +
                "client_secret=" + "HP3RmkgAmEGro0gn1x9ioawQE8WMfvLXDz3ZqxpK" + "&" +
                "grant_type=refresh_token" + "&" +
                "refresh_token=" + refreshToken;
            }

            string response;
            try
            {
                response = HttpHelper.POST(LOGIN_URL, postData);
            } catch
            {
                return false;
            }

            var reader = new JsonTextReader(new StringReader(response));
            while (reader.Read())
            {
                switch (reader.Path)
                {
                    case "response.access_token":
                        AccessToken = reader.ReadAsString();
                        break;
                    case "response.refresh_token":
                        RefreshToken = reader.ReadAsString();
                        break;
                    case "response.user.profile_image_urls.px_170x170":
                        AvatarUrl = reader.ReadAsString();
                        break;
                    case "response.user.id":
                        Id = reader.ReadAsString();
                        break;
                    case "response.user.name":
                        Name = reader.ReadAsString();
                        break;
                    case "response.user.account":
                        Account = reader.ReadAsString();
                        break;
                    case "response.user.is_premium":
                        IsPremium = reader.ReadAsBoolean().Value;
                        break;
                }
            }
            return true;
        }
        internal void LikeIllust(string id)
        {
            HttpHelper.POSTWithAuth(ADD_ILLUST, "illust_id=" + id + "&restrict=public", "Bearer " + this.AccessToken);
        }
        public Task LikeIllustAsync(string id)
        {
            return Task.Run(() => LikeIllust(id));
        }
        internal void UnlikeIllust(string id)
        {
            HttpHelper.POSTWithAuth(DELETE_ILLUST, "illust_id=" + id + "&restrict=public", "Bearer " + this.AccessToken);
        }
        public Task UnlikeIllustAsync(string id)
        {
            return Task.Run(() => UnlikeIllust(id));
        }

        public List<Illust> GetRecommend()
        {
            var source = HttpHelper.GETWithAuth(RECOMMEDN_URL, "Bearer " + this.AccessToken);
            source = DecodeUnicode(source);

            var reader = new JsonTextReader(new StringReader(source));
            List<Illust> r = new List<Illust>();
            var illust = new Illust();
            var index = 0;

            while (reader.Read())
            {
                var march = Regex.Match(reader.Path, @"(?<=illusts\[(\d+?)\]\.).+");
                if(march.Groups[1].Value != "" && march.Groups[1].Value != index.ToString())
                {
                    index++;
                    r.Add(illust);
                    illust = new Illust();
                }
                switch (march.Value)
                {
                    case "id":
                        illust.Id = reader.ReadAsString();
                        break;
                    case "image_urls.square_medium":
                        illust.ImageUrl = reader.ReadAsString();
                        illust.ImageUrl.Replace(@"\/", "/");
                        break;
                }
            }
            return r;
        }
        string DecodeUnicode(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => {
                    if(m.Groups["Value"].Value == @"0022")
                        return @"\""";
                    return ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString();
                });
        }
        public Task<List<Illust>> GetRecommendAsync()
        {
            return Task.Run(() => GetRecommend());
        }
    }
}
