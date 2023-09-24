using WPFAppFrameWork.Common;

namespace WPFAppFrameWork.Calculator.Model
{
	public class CalculatableModel : BindableBase, ICalculatable
	{
		#region フィールド

		/// <summary>
		/// 演算結果
		/// </summary>
		private double? m_Result = 0.0;

		/// <summary>
		/// 演算状態
		/// </summary>
		private CalculateKind m_CalculateState = CalculateKind.None;

		#endregion

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
		public double? Result
		{
			get
			{
				return m_Result;
			}
			private set
			{
				SetProperty(ref m_Result, value);
			}
		}

		/// <summary>
		/// 演算状態
		/// </summary>
		public CalculateKind CalculateState
		{
			get
			{
				return m_CalculateState;
			}
			set
			{
				SetProperty(ref m_CalculateState, value);
			}
		}

		/// <summary>
		/// 演算可能か
		/// </summary>
		/// <remarks>演算種別がなしの場合は不可</remarks>
		public bool CanCalculate
		{
			get
			{
				return m_CalculateState != CalculateKind.None
					&& Operand1 != null && Operand2 != null;
			}
		}

		#endregion

		#region 公開緒サービス

		/// <summary>
		/// 計算する
		/// </summary>
		public void Calculate()
		{
			if (Operand1 == null || Operand2 == null)
			{
				return;
			}

			switch (m_CalculateState)
			{
				case CalculateKind.Add:
					Add(Operand1, Operand2);
					break;
				case CalculateKind.Sub:
					Sub(Operand1, Operand2);
					break;
				case CalculateKind.Multi:
					Multi(Operand1, Operand2);
					break;
				case CalculateKind.Div:
					Div(Operand1, Operand2);
					break;
				default:
					break;
			}

			m_CalculateState = CalculateKind.None;
			// 演算結果をオペランド1に追加して連続して演算子を指定できるようにする
			Operand1 = Result;
			Operand2 = null;
		}

		/// <summary>
		/// クリアする
		/// </summary>
		public void Clear()
		{
			Result = null;
			Operand1 = null;
			Operand2 = null;
			CalculateState = CalculateKind.None;
		}
		#endregion

		#region 内部処理

		#region 加算

		private void Add(double? x, double? y)
		{
			Result = x + y;
		}

		#endregion

		#region 除算

		private void Div(double? x, double? y)
		{
			Result = x / y;
		}

		#endregion

		#region 乗算

		private void Multi(double? x, double? y)
		{
			Result = x * y;
		}

		#endregion

		#region 減算

		private void Sub(double? x, double? y)
		{
			Result = x - y;
		}

		#endregion

		#endregion
	}

	/// <summary>
	/// 演算種別
	/// </summary>
	public enum CalculateKind
	{
		//無し
		None,
		//加算
		Add,
		//減算
		Sub,
		//乗算
		Multi,
		//除算
		Div,
	}
}
