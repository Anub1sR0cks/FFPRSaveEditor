using FFPRSaveEditor.Common;
using FFPRSaveEditor.SaveFiles;
using System.Text.Json;

namespace FFPRSaveEditor.SaveFilesUnitTests
{
    [TestClass]
    public class FF1SaveTests
    {
        [TestMethod]
        public void TestEdit()
        {
            string fileData = File.ReadAllText("TestFiles/FF1/7nCxyzTwG31W3Zlg70mo751W8ETH1n+Km0dWOzRU84Y=");
            string jsonData = SaveGame.Decrypt(fileData);
            var save = FF1Save.Deserialize(jsonData);
            Assert.IsNotNull(save);
            var outputJson = save.Serialize();
            Assert.AreEqual(jsonData.Length, outputJson.Length);
        }
    }
}