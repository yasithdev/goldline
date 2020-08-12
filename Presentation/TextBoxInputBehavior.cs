using System;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Presentation
{
    public class TextBoxInputBehavior : Behavior<TextBox>
    {
        private const NumberStyles ValidNumberStyles = NumberStyles.AllowDecimalPoint
                                                       | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign;


        public static readonly DependencyProperty JustPositivDecimalInputProperty =
            DependencyProperty.Register("JustPositivDecimalInput", typeof (bool),
                typeof (TextBoxInputBehavior), new FrameworkPropertyMetadata(false));

        /* to allow other settings also
                    const NumberStyles ValidNumberStyles = NumberStyles.AllowDecimalPoint 
            |NumberStyles.AllowThousands |NumberStyles.AllowLeadingSign;
        */

        public TextBoxInputBehavior()
        {
            InputMode = TextBoxInputMode.None;
            JustPositivDecimalInput = false;
        }

        public TextBoxInputMode InputMode { get; set; }

        public bool JustPositivDecimalInput
        {
            get { return (bool) GetValue(JustPositivDecimalInputProperty); }
            set { SetValue(JustPositivDecimalInputProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += AssociatedObjectPreviewTextInput;
            AssociatedObject.PreviewKeyDown += AssociatedObjectPreviewKeyDown;

            DataObject.AddPastingHandler(AssociatedObject, Pasting);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= AssociatedObjectPreviewTextInput;
            AssociatedObject.PreviewKeyDown -= AssociatedObjectPreviewKeyDown;

            DataObject.RemovePastingHandler(AssociatedObject, Pasting);
        }

        private void Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof (string)))
            {
                var pastedText = (string) e.DataObject.GetData(typeof (string));

                if (!IsValidInput(GetText(pastedText)))
                {
                    SystemSounds.Beep.Play();
                    e.CancelCommand();
                }
            }
            else
            {
                SystemSounds.Beep.Play();
                e.CancelCommand();
            }
        }

        private void AssociatedObjectPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (!IsValidInput(GetText(" ")))
                {
                    SystemSounds.Beep.Play();
                    e.Handled = true;
                }
            }
        }

        private void AssociatedObjectPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsValidInput(GetText(e.Text))) return;
            //System.Media.SystemSounds.Beep.Play();
            e.Handled = true;
        }

        private string GetText(string input)
        {
            var txt = AssociatedObject;

            var selectionStart = txt.SelectionStart;
            if (txt.Text.Length < selectionStart)
                selectionStart = txt.Text.Length;

            var selectionLength = txt.SelectionLength;
            if (txt.Text.Length < selectionStart + selectionLength)
                selectionLength = txt.Text.Length - selectionStart;

            var realtext = txt.Text.Remove(selectionStart, selectionLength);

            var caretIndex = txt.CaretIndex;
            if (realtext.Length < caretIndex)
                caretIndex = realtext.Length;

            var newtext = realtext.Insert(caretIndex, input);

            return newtext;
        }

        private bool IsValidInput(string input)
        {
            switch (InputMode)
            {
                case TextBoxInputMode.None:
                    return true;
                case TextBoxInputMode.DigitInput:
                    return CheckIsDigit(input);

                case TextBoxInputMode.DecimalInput:
                    decimal d;

                    //if (input.ToCharArray().Where(x => x == ',').Count() > 1)
                    //    return false;

                    if (input.Contains(','))
                        return false;

                    // To have a leading sign (negative sign)
                    //if (input.Contains("-"))
                    //{
                    //    if (this.JustPositivDecimalInput)
                    //        return false;


                    //    if (input.IndexOf("-", StringComparison.Ordinal) > 0)
                    //        return false;

                    //    if (input.ToCharArray().Count(x => x == '-') > 1)
                    //        return false;

                    //    //minus einmal am anfang zulässig
                    //    if (input.Length == 1)
                    //        return true;
                    //}

                    var result = decimal.TryParse(input, ValidNumberStyles, CultureInfo.CurrentCulture, out d);
                    return result;


                default:
                    throw new ArgumentException("Unknown TextBoxInputMode");
            }
        }

        private bool CheckIsDigit(string wert)
        {
            return wert.ToCharArray().All(char.IsDigit);
        }
    }

    public enum TextBoxInputMode
    {
        None,
        DecimalInput,
        DigitInput
    }
}