using WpfAppForLearning.ViewModel;

namespace WpfAppForLearning.Modules.DragDropControl
{
	internal class DragDropControlViewModel : ContentViewModel
	{
		#region 構築・消滅

		public DragDropControlViewModel()
			: base(null)
		{
			Description = "UIElementを継承するコントロールでAllowDropプロパティをtrueにすることで、そのコントロールにオブジェクトをドロップできるようになる。\r\n"
				+ "ドロップしたときには、Dropイベントのイベントハンドラで処理する。\r\n"
				+ "ドロップ前に何か処理したい場合には、PreviewDragOverイベントのイベントハンドラで処理する。";
		}

		#endregion
	}
}
