using DevelopmentCommon.Common;
using WpfCustomControlLibrary.Calculator.Model;

namespace WpfCustomControlLibrary.Calculator.ViewModel
{
	/// <summary>
	/// 計算モジュールのビューモデル
	/// </summary>
	public class CalculatorViewModel : ViewModelBase
	{
		#region フィールド

		/// <summary>
		/// 計算可能なモデル
		/// </summary>
		private CalculatableModel m_CalculatableModel;

		/// <summary>
		/// 入力内容
		/// </summary>
		private string m_InputedString;

		/// <summary>
		/// 入力されたオペランド
		/// </summary>
		private string m_InputedOperand;

		#endregion

		#region プロパティ

		/// <summary>
		/// 入力内容
		/// </summary>
		public string InputedString
		{
			get { return m_InputedString; }
			set { SetProperty(ref m_InputedString, value); }
		}

		/// <summary>
		/// 演算する
		/// </summary>
		public DelegateCommand CalculateCommand { get; protected set; }

		/// <summary>
		/// 演算種別を設定する
		/// </summary>
		public DelegateCommand<CalculateKind> ChangeCaluculationKindCommand { get; protected set; }

		/// <summary>
		/// オペランドを設定する
		/// </summary>
		public DelegateCommand<string> SetOperandCommand { get; protected set; }

		public DelegateCommand ClearCommand { get; protected set; }

		#endregion

		#region 構築・消滅

		public CalculatorViewModel()
			: base(null)
		{
			m_CalculatableModel = new CalculatableModel();
			Model = m_CalculatableModel;
		}

		#endregion

		#region 初期化

		/// <summary>
		/// コマンドの初期化
		/// </summary>
		protected override void RegisterCommands()
		{
			base.RegisterCommands();
			CalculateCommand = new DelegateCommand(Calculate, CanCalculate);
			ChangeCaluculationKindCommand = new DelegateCommand<CalculateKind>(ChangeCaluculationKind, CanChangeCaluculationKind);
			SetOperandCommand = new DelegateCommand<string>(SetOperand, CanSetOperand);
			ClearCommand = new DelegateCommand(Clear, CanClear);

		}

		#endregion

		#region コマンド

		#region 計算する

		/// <summary>
		/// 計算可能か
		/// </summary>
		/// <returns>計算可能な場合は真</returns>
		private bool CanCalculate()
		{
			return m_CalculatableModel.CanCalculate;
		}

		/// <summary>
		/// 計算する
		/// </summary>
		private void Calculate()
		{
			//計算する
			m_CalculatableModel.Calculate();
			InputedString = m_CalculatableModel.Result.ToString();
			m_InputedOperand = InputedString;
		}

		#endregion

		#region 計算種別を変更する

		/// <summary>
		/// 計算種別を変更可能か
		/// </summary>
		/// <param name="parameter">計算種別</param>
		/// <returns>変更可能な場合は真</returns>
		private bool CanChangeCaluculationKind(CalculateKind parameter)
		{
			return (parameter == CalculateKind.Sub && string.IsNullOrEmpty(m_InputedOperand)) || (m_CalculatableModel.CalculateState == CalculateKind.None && !string.IsNullOrEmpty(m_InputedOperand));
		}

		/// <summary>
		/// 計算種別を変更する
		/// </summary>
		/// <param name="parameter"></param>
		private void ChangeCaluculationKind(CalculateKind parameter)
		{
			InputedString += " " + TanslateOperator(parameter) + " ";
			//最初の入力が減算記号の場合には、負数を表す演算子と判定するため、計算対象演算子を更新しない
			if(string.IsNullOrEmpty(m_InputedOperand) && parameter == CalculateKind.Sub)
			{
				m_InputedOperand = TanslateOperator(parameter);
				return;
			}
			m_InputedOperand = "";
			m_CalculatableModel.CalculateState = parameter;
		}

		#endregion

		#region オペランドを設定する

		/// <summary>
		/// オペランドを設定可能か
		/// </summary>
		/// <param name="param">設定対象値の文字列</param>
		/// <returns>可能な場合は真</returns>
		private bool CanSetOperand(string param)
		{
			return true;
		}

		/// <summary>
		/// オペランドを設定する
		/// </summary>
		/// <param name="param">設定対象値</param>
		private void SetOperand(string param)
		{
			m_InputedOperand += param;
			double.TryParse(m_InputedOperand, out var result);
			if(m_CalculatableModel.CalculateState == CalculateKind.None)
			{
				m_CalculatableModel.Operand1 = result;
			}
			else
			{
				m_CalculatableModel.Operand2 = result;
			}
			
			InputedString += param;
		}

		#endregion

		#region クリアする

		private bool CanClear()
		{
			return m_CalculatableModel.Operand1 != null ;
		}

		private void Clear()
		{
			m_CalculatableModel.Clear();
			InputedString = "";
			m_InputedOperand = "";
		}
		#endregion

		/// <summary>
		/// 演算種別を演算記号に変換する
		/// </summary>
		/// <param name="kind">演算種別</param>
		/// <returns></returns>
		private string TanslateOperator(CalculateKind kind)
		{
			string ret="";
			switch (kind)
			{
				case CalculateKind.Add:
					ret = @"+";
					break;
				case CalculateKind.Sub:
					ret = @"-";
					break;
				case CalculateKind.Multi:
					ret = @"*";
					break;
				case CalculateKind.Div:
					ret = @"/";
					break;
				default:
					break;
			}

			return ret;
		}

		#endregion
	}
}
