using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProyectoRF
{
    public class FaceRecognitionService
    {
        private readonly IFaceClient faceClient;

        public FaceRecognitionService(string endpoint, string apiKey)
        {
            faceClient = new FaceClient(new ApiKeyServiceClientCredentials(apiKey)) { Endpoint = endpoint };
        }

        public async Task<IList<DetectedFace>> DetectedFacesAsync(Stream imageStream)
        {
            try
            {
                var detectedFaces = await faceClient.Face.DetectWithStreamAsync(imageStream);
                return detectedFaces;
            }
            catch (APIErrorException ex)
            {
                
                throw new InvalidOperationException($"Error en la API de Face: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                
                throw new InvalidOperationException($"Error al intentar detectar rostros: {ex.Message}", ex);
            }
        }
    }
}
