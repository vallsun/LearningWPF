namespace WPFAppFrameWork.Calculator.Model
{
	/// <summary>
	/// 算術演算可能なモデル
	/// </summary>
	public interface ICalculatable
	{
		#region プロパティ

		/// <summary>
		/// オペランド1
		/// </summary>
		public double? Operand1 { get; set; }

		/// <summary>
		/// オペランド2
		/// </summary>
		public double? Operand2 { get; set; }

		/// <summary>
		/// 演算結果
		/// </summary>
		public double? Result { get; }

		/// <summary>
		/// 演算状態
		/// </summary>
		public CalculateKind CalculateState { get; set; }

		/// <summary>
		/// 演算可能か
		/// </summary>
		/// <remarks>演算種別がなしの場合は不可</remarks>
		public bool CanCalculate { get; }

		#endregion

		#region 公開サービス

		/// <summary>
		/// 計算する
		/// </summary>
		void Calculate();

		/// <summary>
		/// クリアする
		/// </summary>
		public void Clear();

		#endregion
	}
}
