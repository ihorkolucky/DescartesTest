using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;

namespace DescartesTest.Controllers
{
    [ApiController]
    public class DescartesTestController : ControllerBase
    {
        private readonly ILogger<DescartesTestController> _logger;

        private static Dictionary<string, Diff> _dictionary = new Dictionary<string, Diff>();

        public DescartesTestController(ILogger<DescartesTestController> logger)
        {
            // not used

            _logger = logger;
        }

        [HttpGet("v1/diff/{ID}")]
        public ActionResult Diff(string ID)
        {
            // get diff object from session
            Diff? diff = GetFromSession(ID);

            // check diff object, if it's not or missing left/right return 404
            if (diff == null || !diff.IsFilled)
            {
                return NotFound();
            }

            DiffResult diffResult = new DiffResult(diff);

            var jObject = diffResult.GetResult();

            return Ok(jObject.ToString());
        }

        [HttpPut("v1/diff/{ID}/left")]
        public ActionResult Left(string ID, [FromBody] JsonElement json)
        {

            // read passed data
            var data = json.GetProperty("data").GetString();

            // if "data": null, return bad request 400
            if (data == null)
            {
                return BadRequest();
            }

            // decode to normal string
            string decodedString = DecodeBase64(data ?? "");

            // try to get diff object from session
            Diff? diff = GetFromSession(ID);

            // if there is no diff object in session, create it and fill left, or just fill the left
            if (diff == null)
            {
                diff = new Diff { Left = decodedString };
            }
            else
            {
                diff.Left = decodedString;
            }

            // add diff object to session
            AddToSession(ID, diff);

            // return created status
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut("v1/diff/{ID}/right")]
        public ActionResult Right(string ID, [FromBody] JsonElement json)
        {
            // read passed data
            var data = json.GetProperty("data").GetString();

            // if "data": null, return bad request 400
            if (data == null)
            {
                return BadRequest();
            }

            // decode to normal string
            string decodedString = DecodeBase64(data ?? "");

            // try to get diff object from session
            Diff? diff = GetFromSession(ID);

            // if there is no diff object in session, create it and fill left, or just fill the right
            if (diff == null)
            {
                diff = new Diff { Right = decodedString };
            }
            else
            {
                diff.Right = decodedString;
            }

            AddToSession(ID, diff);

            return StatusCode((int)HttpStatusCode.Created);
        }

        #region helpers

        /// <summary>
        /// Returns Diff object from Session by Id
        /// </summary>
        /// <param name="id">Id param</param>
        /// <returns>Diff object</returns>
        private Diff? GetFromSession(string id)
        {
            if (_dictionary.ContainsKey(id))
            {
                return _dictionary[id];
            }

            return null;

            //string? diffStr = HttpContext.Session.GetString(id);
            //if (diffStr == null)
            //{
            //    return null;
            //}

            //var diff = JsonConvert.DeserializeObject<Diff>(diffStr);

            //return diff;

        }

        /// <summary>
        /// Adds Diff object to session
        /// </summary>
        /// <param name="id">Id param</param>
        /// <param name="diff">Diff object param</param>
        private void AddToSession(string id, Diff diff)
        {
            if (_dictionary.ContainsKey(id))
            {
                _dictionary[id] = diff;
            }
            else
            {
                _dictionary.Add(id, diff);
            }
            //var diffStr = JsonConvert.SerializeObject(diff);
            //HttpContext.Session.SetString(id, diffStr);
        }

        /// <summary>
        /// decodes base64 string to normal string
        /// </summary>
        /// <param name="base64string">base 64 string</param>
        /// <returns>normal string</returns>
        private string DecodeBase64(string base64string)
        {
            byte[] data = Convert.FromBase64String(base64string);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        #endregion
    }
}
