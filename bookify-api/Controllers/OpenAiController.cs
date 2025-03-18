using bookify_data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenAI.Images;

namespace bookify_api.Controllers
{
	[ApiController]
	[Route("api/v1/open-ai")]
	public class OpenAiController : ControllerBase
	{
		private readonly OpenAIConfig _openAIConfig;

		public OpenAiController(IOptions<OpenAIConfig> openAiOptions)
		{
			_openAIConfig = openAiOptions.Value;
		}

		// GET: /api/OpenAi/generateImage?input=...
		[HttpGet("generateImage")]
		public async Task<IActionResult> GenerateImage([FromQuery] string input)
		{
			var apiKey = _openAIConfig.ApiKeyVip;

			var imageClient = new ImageClient("dall-e-3", apiKey);

			var generateOption = new ImageGenerationOptions
			{
				Quality = GeneratedImageQuality.High,
				Size = GeneratedImageSize.W1024xH1024,
				Style = GeneratedImageStyle.Natural,
				ResponseFormat = GeneratedImageFormat.Uri
			};

			var response = await imageClient.GenerateImageAsync(input, generateOption);
			return Ok(response.Value.ImageUri);
		}

		// POST: /api/OpenAi/editGeneratedImage
		[HttpPost("editGeneratedImage")]
		public async Task<IActionResult> EditGeneratedImage()
		{
			var apiKey = _openAIConfig.ApiKeyVip;

			var imageClient = new ImageClient("dall-e-3", apiKey);

			var imageRequest = new ImageVariationOptions
			{
				Size = GeneratedImageSize.W1024xH1024,
				ResponseFormat = GeneratedImageFormat.Uri
			};

			var response = await imageClient.GenerateImageVariationsAsync("./images/art.png", 1, imageRequest);
			// response.Value là mảng, trả về phần tử đầu tiên
			return Ok(response.Value[0].ImageUri);
		}

		// POST: /api/OpenAi/GenerateRandomImageBasedOnFile
		[HttpPost("GenerateRandomImageBasedOnFile")]
		public async Task<IActionResult> GenerateRandomImageBasedOnFile()
		{
			var imageClient = new ImageClient("dall-e-2", "<get API key from OpenAI portal>");
			// Thực hiện logic tùy ý, hiện tại trả về rỗng
			return Ok(new { });
		}

		
	}
}
