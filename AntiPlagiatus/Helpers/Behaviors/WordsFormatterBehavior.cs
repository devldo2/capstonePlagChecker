using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace AntiPlagiatus.Helpers.Behaviors
{
    public class WordsFormatterBehavior : Behavior<TextBlock>
    {
        public string OriginalText
        {
            get { return (string)GetValue(OriginalTextProperty); }
            set { SetValue(OriginalTextProperty, value); }
        }

        public static readonly DependencyProperty OriginalTextProperty =
            DependencyProperty.Register("OriginalText", typeof(string), typeof(WordsFormatterBehavior), new PropertyMetadata(string.Empty));

        public List<int> Words
        {
            get { return (List<int>)GetValue(WordsProperty); }
            set { SetValue(WordsProperty, value); }
        }

        public static readonly DependencyProperty WordsProperty =
            DependencyProperty.Register("Words", typeof(List<int>), typeof(WordsFormatterBehavior), new PropertyMetadata(null, WordsCollectionChanged));

        public SolidColorBrush WordsColor
        {
            get { return (SolidColorBrush)GetValue(WordsColorProperty); }
            set { SetValue(WordsColorProperty, value); }
        }

        public static readonly DependencyProperty WordsColorProperty =
            DependencyProperty.Register("WordsColor", typeof(SolidColorBrush), typeof(WordsFormatterBehavior), new PropertyMetadata(Colors.Black));


        private static void WordsCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as WordsFormatterBehavior;
            if (behavior != null && behavior.AssociatedObject != null)
            {
                if (behavior.AssociatedObject.Inlines?.Count > 0)
                    behavior.AssociatedObject.Inlines.Clear();

                var newItems = e.NewValue as List<int>;
                if (newItems?.Count > 0 && !string.IsNullOrEmpty(behavior.OriginalText))
                {
                    newItems = newItems.ToList();
                    var text = behavior.OriginalText;
                    if (!string.IsNullOrEmpty(text))
                    {
                        var runText = string.Empty;
                        var textWord = string.Empty;
                        var currentWordStartPos = 0;
                        var neededWordStartPos = newItems.First();
                        for (int i = 0; i < text.Length; i++)
                        {
                            if (!char.IsPunctuation(text[i]) && text[i] != ' ')
                                textWord += text[i];
                            else
                            {
                                if (currentWordStartPos != neededWordStartPos)
                                    runText += textWord + text[i];
                                else
                                {
                                    if (text[i] == ' ')
                                        textWord += text[i];
                                    if (!string.IsNullOrEmpty(runText))
                                        behavior.AssociatedObject.Inlines.Add(new Run() { Text = runText });
                                    behavior.AssociatedObject.Inlines.Add(new Run() { Text = textWord, FontWeight= FontWeights.Bold, Foreground = behavior.WordsColor });
                                    runText = string.Empty;
                                    if (newItems.Count > 0)
                                    {
                                        newItems.RemoveAt(0);
                                        if (newItems.Count == 0)
                                        {
                                            runText = text.Substring(i);
                                            if (!string.IsNullOrEmpty(runText))
                                                behavior.AssociatedObject.Inlines.Add(new Run() { Text = runText });
                                            break;
                                        }
                                        else
                                            neededWordStartPos = newItems.First();
                                    }
                                }

                                if (text[i] == ' ')
                                    currentWordStartPos++;
                                textWord = string.Empty;
                            }
                        }
                    }
                }
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
