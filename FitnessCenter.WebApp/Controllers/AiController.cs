using Microsoft.AspNetCore.Mvc;
using System.Net; 

namespace FitnessCenter.WebApp.Controllers
{
    public class AiController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Chat(string message, IFormFile? imageFile, string gender, int height, int weight)
        {
            try
            {

                if (height == 0) height = 175;
                if (weight == 0) weight = 75;

                double heightInMeters = height / 100.0;
                double bmi = weight / (heightInMeters * heightInMeters);

                string aiAnalysisText = GenerateSmartProgram(gender, bmi, height, weight);

                string finalImageUrl = GenerateFreeAiImage(gender, bmi);

                return Json(new
                {
                    reply = aiAnalysisText,
                    imageUrl = finalImageUrl,
                    isImage = !string.IsNullOrEmpty(finalImageUrl)
                });
            }
            catch (Exception ex)
            {
                return Json(new { reply = "Hata oluştu: " + ex.Message, isImage = false });
            }
        }

        private string GenerateFreeAiImage(string gender, double bmi)
        {
            try
            {
                string bodyDescription = "";
                if (bmi > 25) bodyDescription = "strong bulky bodybuilder physique, powerful muscles";
                else if (bmi < 18.5) bodyDescription = "lean athletic physique, aesthetic muscles";
                else bodyDescription = "perfect fitness model physique, defined muscles";
                string genderTerm = "";
                string clothing = "";

                if (gender == "Male")
                {
                    genderTerm = "handsome man";
                    clothing = "wearing a loose t-shirt and gym shorts, male sportswear";
                }
                else
                {
                    genderTerm = "beautiful woman";
                    clothing = "wearing a modest long-sleeve sports jersey and long athletic leggings, fully clothed, conservative sportswear, high neck t-shirt, no skin exposure";
                }

                string prompt = $"full body shot of a {bodyDescription} {genderTerm} in a modern gym, {clothing}, cinematic lighting, 8k resolution, photorealistic, masterpiece";

                Random rnd = new Random();
                int seed = rnd.Next(1, 999999);

                string apiUrl = $"https://image.pollinations.ai/prompt/{prompt}?width=1024&height=1024&seed={seed}&nologo=true&model=flux&private=true";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                    var imageBytes = client.GetByteArrayAsync(apiUrl).Result;
                    return "data:image/jpeg;base64," + Convert.ToBase64String(imageBytes);
                }
            }
            catch
            {
                return "";
            }
        }

        private string GenerateSmartProgram(string gender, double bmi, int height, int weight)
        {
            string text = $"📊 **VÜCUT ANALİZ RAPORU**\n";
            text += $"• Boy: {height} cm | Kilo: {weight} kg\n";
            text += $"• Vücut Kitle İndeksi (BMI): **{bmi:F1}**\n";

            string status = bmi < 18.5 ? "Zayıf" : (bmi < 25 ? "İdeal" : "Kilolu");
            text += $"• Durum: **{status}**\n\n";

            text += "🎯 **HEDEFİN:**\n";
            if (status == "Zayıf") text += "Hacim kazanarak (Clean Bulk) kas kütlesini artırmak.\n\n";
            else if (status == "Kilolu") text += "Yağ oranını düşürerek (Definasyon) kasları belirginleştirmek.\n\n";
            else text += "Mevcut formu koruyarak atletik performansı artırmak.\n\n";

            text += "🔥 **4 GÜNLÜK ANTRENMAN PLANI**\n";
            text += "--------------------------------\n";

            if (gender == "Male")
            {
                text += "**1. Gün (Göğüs & Ön Kol):**\n• Bench Press (4x10)\n• Incline Dumbbell Press (4x12)\n• Barbell Curl (3x12)\n\n";
                text += "**2. Gün (Sırt & Arka Kol):**\n• Lat Pulldown (4x12)\n• Seated Row (4x12)\n• Triceps Pushdown (4x15)\n\n";
                text += "**3. Gün (Omuz & Bacak):**\n• Overhead Press (4x10)\n• Squat (4x8)\n• Leg Extension (3x15)\n\n";
                text += "**4. Gün (Full Body & Cardio):**\n• Deadlift (3x8)\n• 20dk HIIT Kardiyo";
            }
            else
            {
                text += "**1. Gün (Kalça & Bacak):**\n• Hip Thrust (4x12)\n• Squat (4x12)\n• Lunges (3x15)\n\n";
                text += "**2. Gün (Üst Vücut & Karın):**\n• Shoulder Press (3x12)\n• Lat Pulldown (3x12)\n• Plank (3x1dk)\n\n";
                text += "**3. Gün (Tüm Vücut):**\n• Deadlift (3x10)\n• Push Up (3xMax)\n• Mountain Climber (3x20)\n\n";
                text += "**4. Gün (Cardio & Esneme):**\n• 30dk Tempolu Yürüyüş\n• 15dk Yoga/Esneme";
            }

            text += "\n\n🥗 **BESLENME TAVSİYESİ:**\n";
            if (status == "Kilolu") text += "Kalori açığı oluştur. Şekeri tamamen kes, akşam 7'den sonra karbonhidrat alma. Bol su iç.";
            else if (status == "Zayıf") text += "Kalori fazlası al. Günde 3 ana, 2 ara öğün yap. Fıstık ezmesi, yulaf ve kırmızı et tüket.";
            else text += "Yüksek proteinli beslen. İşlenmiş gıdalardan uzak dur, antrenman sonrası protein shake içmeyi unutma.";

            return text;
        }
    }
}