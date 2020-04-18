﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using DiffPlex.DiffBuilder.Model;
using FileRenamerDiff.Models;

namespace FileRenamerDiff.Views
{
    public class DiffPaneModelToFlowDocumentConverter : IValueConverter
    {
        //見やすいように少し半透明にしておく
        private static readonly Brush unchangeBrush = new SolidColorBrush(Colors.Transparent).ToFreeze();
        private static readonly Brush deletedBrush = new SolidColorBrush(AppExtention.CodeToColorOrTransparent($"#FFAFD1")).ToFreeze();
        private static readonly Brush insertedBrush = new SolidColorBrush(AppExtention.CodeToColorOrTransparent($"#88E6A7")).ToFreeze();
        private static readonly Brush imaginaryBrush = new SolidColorBrush(Colors.SkyBlue).ToFreeze();
        private static readonly Brush modifiedBrush = new SolidColorBrush(Colors.Orange).ToFreeze();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is DiffPaneModel diffVM))
                return Binding.DoNothing;

            if (!diffVM.Lines.Any())
                return Binding.DoNothing;

            if (diffVM.Lines.Count > 1)
                Debug.WriteLine($"Warning Lines {diffVM.Lines.Count > 1}");

            List<Run> lineView = ConvertLinveVmToRuns(diffVM.Lines.First());

            var paragraph = new Paragraph();
            paragraph.Inlines.AddRange(lineView);
            return new FlowDocument(paragraph);
        }

        private static List<Run> ConvertLinveVmToRuns(DiffPiece lineVM) =>
            lineVM.Type switch
            {
                //ChangeType.Modifiedだったら変更された部分だけハイライトしたいのでSubPieceからいろいろやる
                ChangeType.Modified => lineVM
                    .SubPieces
                    .Select(x => ConvertPieceVmToRun(x))
                    .ToList(),

                //ChangeType.Modified以外は行全体で同じ書式
                _ => new List<Run> { ConvertPieceVmToRun(lineVM) },
            };

        private static Run ConvertPieceVmToRun(DiffPiece pieceVM) =>
            new Run
            {
                Text = pieceVM.Text,
                //差分タイプによって、背景色を決定
                Background = (pieceVM.Type switch
                {
                    ChangeType.Deleted => deletedBrush,
                    ChangeType.Inserted => insertedBrush,
                    ChangeType.Imaginary => imaginaryBrush,
                    ChangeType.Modified => modifiedBrush,
                    _ => unchangeBrush
                }),
            };

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}