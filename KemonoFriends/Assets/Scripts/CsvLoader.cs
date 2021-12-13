// BEGIN MIT LICENSE BLOCK //
//
// Copyright (c) 2016 dskjal
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//
// END MIT LICENSE BLOCK   //

// ******************************使い方*************************************
// csv のロード（Resources/test.csv を読み込む場合）
// var csv = CSVLoader.Load("test.csv");

// 内容の出力
// foreach(var t in csv){
//     Debug.Log(t);
// }

// データにアクセス
// Debug.Log(csv[2, 1]);
using UnityEngine;
using System.Collections.Generic;

public static class CSVLoader
{
    public class CSV
    {
        public CSV()
        {
            rows = new List<List<string>>();
        }
        public int GetColumnCount() { return rows.Count > 0 ? rows[0].Count : 0; }
        public int GetRowCount() { return rows.Count; }
        public List<string> GetRow(int i)
        {
            if(i >= rows.Count)
            {
                throw new System.Exception("無効な列が指定されました．");
            }
            return rows[i];
        }
        public string this[int row, int column]
        {
            get { return this.rows[row][column]; }
        }
        public IEnumerator<string> GetEnumerator()
        {
            foreach(var column in rows)
            {
                foreach(var t in column)
                {
                    yield return t;
                }
            }
        }

        internal List<List<string>> rows;
    }

    private static List<string> getColumn(string text, int startPos, out int endPos)
    {
        var column = new List<string>();

        int elemStart = startPos;
        int i = startPos;
        bool isContinue = true;
        while(isContinue)
        {
            switch(text[i])
            {
            case '"':
                ++elemStart;
                // 対応する " まで読み込む
                while(++i < text.Length)
                {
                    if(text[i] == '"')
                    {
                        break;
                    }
                }
                break;
            case '\n':
                isContinue = false;
                goto case ',';
            case ',':
                // クォート時の文字列の長さの調整 フォールスルーがあるため改行もチェック
                var offset = 1;
                while(text[i - offset] == '"' || text[i - offset] == '\r' || text[i - offset] == '\n')
                {
                    ++offset;
                }
                column.Add(text.Substring(elemStart, i - elemStart - offset + 1));
                elemStart = i + 1;
                break;
            }
            ++i;
        }

        endPos = i;
        return column;
    }

    // Resources フォルダから読み込む場合はこちらを使う
    public static CSV LoadFromResources(string path)
    {
        var ta = GameUtility.Load<TextAsset>(path);
        if(ta == null)
        {
            throw new System.Exception("CSV ファイル：" + path + "が見つかりません．");
        }

        var csv = new CSV();
        using(var sr = new System.IO.StringReader(ta.text))
        {
            var text = sr.ReadToEnd();

            for(int i = 0; i < text.Length;)
            {
                csv.rows.Add(getColumn(text, i, out i));
            }
        }

        return csv;
    }

    // Resources ファイル以外の場所から読み込む場合はこちらを使う
    public static CSV Load(string path)
    {
        var csv = new CSV();
        using(var sr = new System.IO.StreamReader(Application.dataPath + path, System.Text.Encoding.GetEncoding("Utf-8")))
        {
            var text = sr.ReadToEnd();

            for(int i = 0; i < text.Length;)
            {
                csv.rows.Add(getColumn(text, i, out i));
            }
        }

        return csv;
    }
}