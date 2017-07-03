using Android.App;
using Android.Widget;
using Android.OS;
using HackAtHome.SAL;
using HackAtHome.Entities;

namespace HackAtHomeClient
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/Icon")]
    public class MainActivity : Activity
    {
        EditText txtEmailValue;
        EditText txtPasswordValue;
        Button btnValidate;
        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);            
            SetContentView (Resource.Layout.Main);

            txtEmailValue = FindViewById<EditText>(Resource.Id.txtEmailValue);
            txtPasswordValue = FindViewById<EditText>(Resource.Id.txtPasswordValue);
            btnValidate = FindViewById<Button>(Resource.Id.btnValidate);

            txtEmailValue.Text = "j.kmv@hotmail.com";
            txtPasswordValue.Text = MyPass();

            btnValidate.Click += BtnValidate_Click;
        }

        private void BtnValidate_Click(object sender, System.EventArgs e)
        {
            //Realiza las autenticaciones
            ValidateAuthentication();            
        }

        /// <summary>
        /// Consume el servicio ServiceClient validando la autenticación
        /// </summary>
        /// <returns>Retorna un array con los datos FullName y Token obtenidos</returns>
        private async void ValidateAuthentication()
        {
            string email = txtEmailValue.Text;
            string pass = txtPasswordValue.Text;

            ServiceClient ServiceClient = new ServiceClient();
            ResultInfo Result = await ServiceClient.AutenticateAsync(email,pass);
            

            ValidateMicrosot();

            //Se verifica si la autenticación fue correcta
            if(Result.Status == Status.AllSuccess || Result.Status == Status.Success)
            {
                //Se inicia el Activity enviando los parametros FullName y Token
                var ActivityIntent = new Android.Content.Intent(this, typeof(EvidencesActivity));
                ActivityIntent.PutExtra("FullName", Result.FullName);
                ActivityIntent.PutExtra("Token", Result.Token);
                StartActivity(ActivityIntent);
            }
            
        }

        /// <summary>
        /// Consume servicio MicrosoftServiceClient enviando la evidencia
        /// </summary>
        private async void ValidateMicrosot()
        {
            string email = txtEmailValue.Text;
            var MicrosoftEvidence = new LabItem
            {
                Email = email,
                Lab = "Hack@Home",
                DeviceId = Android.Provider.Settings.Secure.GetString
                (
                    ContentResolver,
                    Android.Provider.Settings.Secure.AndroidId
                )
            };

            MicrosoftServiceClient MicrosoftClient = new MicrosoftServiceClient();
            await MicrosoftClient.SendEvidence(MicrosoftEvidence);
        }

        /// <summary>
        /// Devuelve el password
        /// </summary>
        /// <returns>password (cuidenlo por favor) :c</returns>
        private string MyPass()
        {
            return "ytv79/kev";
        }
    }
}

