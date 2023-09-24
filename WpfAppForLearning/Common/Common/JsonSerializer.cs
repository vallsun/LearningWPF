using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace DevelopmentCommon.Common
{
	public class JsonSerializer
	{
		#region 内部フィールド

		private Encoding m_DefaultEncoding = new UTF8Encoding(false);

		#endregion

		#region 公開サービス

		public void Save<T>(string folder, string fileName, T target)
		{
			if(!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			string filePath = folder + "\\" + fileName;

			var options = new JsonSerializerOptions
			{
				// JavaScriptEncoder.Createでエンコードしない文字を指定するのも可
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
				// 読みやすいようインデントを付ける
				WriteIndented = true
			};
			string jsonData = System.Text.Json.JsonSerializer.Serialize(target, options);

			StreamWriter sw = new StreamWriter(filePath, false, m_DefaultEncoding);
			try
			{
				sw.Write(jsonData);
			}
			finally
			{
				sw.Close();
			}
		}

		#endregion


	}
}
