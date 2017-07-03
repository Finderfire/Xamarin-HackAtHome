using HackAtHome.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;

namespace HackAtHome.SAL
{
    public class ServiceClient
    {
        /// <summary>
        /// Realiza la autenticación al servicio Web API.
        /// </summary>
        /// <param name = "studentEmail">Correo del usuario</param>
        /// <param name = "studentPassword">Password del usuario</param>
        /// <returns>Objeto ResultInfo con los datos del usuario y un token de autenticación</returns>

        public async Task<ResultInfo> AutenticateAsync(string studentEmail, string studentPassword)
        {
            ResultInfo Result = null;

            //Datos consumidos por el cliente Web API
            string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";
            string EventID = "xamarin30";
            string RequestUri = "api/evidence/Authenticate";

            //El servicio requiere un Objeto UserInfo
            UserInfo User = new UserInfo
            {
                Email = studentEmail,
                Password = studentPassword,
                EventID = EventID
            };

            //HttpClient debe instalarse, este permitira consumir el servicio REST
            using (var Client = new HttpClient())
            {
                //Direccion base del servicio REST
                Client.BaseAddress = new Uri(WebAPIBaseAddress);

                //Limpiar encabezados de la peticion
                Client.DefaultRequestHeaders.Accept.Clear();

                //Los datos se enviaran en formato JSON.
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    //Serializamos a formato JSON el objeto User. (se necesita Newtonsoft.Json)
                    var JSONUserInfo = JsonConvert.SerializeObject(User);

                    //Se realiza un POST al servicio enviando el objeto JSON
                    HttpResponseMessage Response = await Client.PostAsync
                        (
                            RequestUri, 
                            new StringContent(JSONUserInfo.ToString(),Encoding.UTF8,"application/json")
                        );

                    //Se lee el resultado de la peticion.
                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();

                    //El resultado en formato JSON debe ser deserializado
                    Result = JsonConvert.DeserializeObject<ResultInfo>(ResultWebAPI);
                }
                catch(System.Exception)
                {
                    //error
                }
            }
            return Result;            
        }

        /// <summary>
        /// Obtiene el detalle de una evidencia.
        /// </summary>
        /// <param name="token">Token de autenticación del usuario</param>
        /// <param name="evidenceID">Identificador de la evidencia.</param>
        /// <returns>Información de la evidencia.</returns>
        public async Task<EvidenceDetail> GetEvidenceByIDAsync(string token, int evidenceID)
        {
            EvidenceDetail Result = null;
            string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";
            // URI de la evidencia.
            string URI = $"{WebAPIBaseAddress}api/evidence/getevidencebyid?token={token}&&evidenceid={evidenceID}";

            using (var Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    // Realizamos una petición GET
                    var Response = await Client.GetAsync(URI);
                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        // Si el estatus de la respuesta HTTP fue exitosa, leemos
                        // el valor devuelto. 

                        Result = JsonConvert.DeserializeObject<EvidenceDetail>(ResultWebAPI);
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return Result;
        }

        /// <summary>
        /// Obtiene la lista de evidencias.
        /// </summary>
        /// <param name="token">Token de autenticación del usuario.</param>
        /// <returns>Una lista con las evidencias.</returns>
        public async Task<List<Evidence>> GetEvidencesAsync(string token)
        {
            List<Evidence> Evidences = null;

            //direccion de la Web API
            string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";
            string URI = $"{WebAPIBaseAddress}api/evidence/getevidences?token={token}";

            using (var Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    //Peticion GET
                    var Response = await Client.GetAsync(URI);

                    //Evalua el status de la respuesta HTTP
                    if(Response.StatusCode == HttpStatusCode.OK)
                    {
                        var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                        Evidences = JsonConvert.DeserializeObject<List<Evidence>>(ResultWebAPI);
                    }
                }
                catch (System.Exception)
                {
                    //error
                }
            }
            return Evidences;
        }

        

    }
}
