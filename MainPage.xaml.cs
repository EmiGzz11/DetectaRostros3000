using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using ProyectoRF;
namespace DetectaRostros3000
{
    public partial class MainPage : ContentPage
    {
        private readonly FaceRecognitionService _faceRecognitionService;



        private const string AzureFaceEndpoint = "END_POINTS";
        private const string AzureFaceApiKey = "YOUR_API_KEY";

        
        private byte[] _uploadedImageBytes;

        public MainPage()
        {
            InitializeComponent();
            _faceRecognitionService = new FaceRecognitionService(AzureFaceEndpoint, AzureFaceApiKey);
        }

        private async void OnUploadImageButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Seleccione una imagen",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    
                    using (var fileStream = await result.OpenReadAsync())
                    using (var memoryStream = new MemoryStream())
                    {
                        await fileStream.CopyToAsync(memoryStream);
                        _uploadedImageBytes = memoryStream.ToArray();
                    }

                    
                    UploadedImage.Source = ImageSource.FromStream(() => new MemoryStream(_uploadedImageBytes));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo cargar la imagen: {ex.Message}", "OK");
            }
        }

        private async void OnDetectFaceButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (_uploadedImageBytes == null)
                {
                    await DisplayAlert("Error", "Por favor, cargue una imagen antes de detectar rostros.", "OK");
                    return;
                }

                
                using (var imageStream = new MemoryStream(_uploadedImageBytes))
                {
                    var faces = await _faceRecognitionService.DetectedFacesAsync(imageStream);

                    if (faces != null && faces.Count > 0)
                    {
                        await DisplayAlert("Resultado", $"Se detectaron {faces.Count} rostros en la imagen.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Resultado", "No se detectaron rostros en la imagen.", "OK");
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Hubo un error al intentar detectar rostros: {ex.Message}", "OK");
            }
        }
    }
}