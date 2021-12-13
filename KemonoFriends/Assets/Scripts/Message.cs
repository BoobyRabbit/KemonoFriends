using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// メッセージを表示するクラス
/// </summary>
public class Message : MonoBehaviour, IManualUpdate
{
    /// <summary>
    /// メッセージ１つ分の情報
    /// </summary>
    private class Item
    {
        /// <summary>
        /// キー
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// メッセージ
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 指定した値で初期化
        /// </summary>
        public Item(string key, string name, string message)
        {
            this.Key = key;
            this.Name = name;
            this.Message = message;
        }
    }

    /// <summary>
    /// メッセージ用エクセルデータの列名.
    /// </summary>
    private enum ColumnName
    {

        /// <summary>
        /// キー
        /// 同じキーのメッセージは順番に表示されます。
        /// </summary>
        Key,

        /// <summary>
        /// 表示する名前
        /// </summary>
        Name,

        /// <summary>
        /// 表示するメッセージ
        /// </summary>
        Message,

        /// <summary>
        /// 動作に影響のないメモ
        /// </summary>
        Memo,

        /// <summary>
        /// 未定義の列名
        /// </summary>
        Invalid,
    }

    /// <summary>
    /// 名前のウィンドウ
    /// </summary>
    [SerializeField]
    private GameObject nameWindow = null;

    /// <summary>
    /// 発言者の名前
    /// </summary>
    [SerializeField]
    private Text nameText = null;

    /// <summary>
    /// メッセージウィンドウ
    /// </summary>
    [SerializeField]
    private GameObject messageWindow = null;

    /// <summary>
    /// メッセージの内容
    /// </summary>
    [SerializeField]
    private Text messageText = null;

    /// <summary>
    /// 次のメッセージへ進むボタン
    /// </summary>
    [SerializeField]
    private string nextButton = string.Empty;

    /// <summary>
    /// メッセージのリスト
    /// </summary>
    private List<Message.Item> Items { get; set; } = new List<Item>();

    /// <summary>
    /// メッセージのリストのインデックス
    /// </summary>
    private int ItemIndex { get; set; } = 0;

    /// <summary>
    /// キー
    /// </summary>
    private string Key { get; set; } = string.Empty;

    /// <summary>
    /// １秒間に表示する文字数
    /// </summary>
    private static readonly int showCharPerSecond = 100;

    /// <summary>
    /// 表示する文字数
    /// </summary>
    private float showChar = 0.0f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        // @todo コツメカワウソ確定で代入していますが、
        // 話しかけたキャラクターのメッセージを代入するよう修正してください。
        //this.Load("Message/TestMessage");
        //this.StartMessage("TestKey");
    }

    public void ManualUpdate()
    {
        var item = CurrentItem();
        if(item != null)
        {
            this.showChar += Time.deltaTime * Message.showCharPerSecond;
            if(this.showChar > item.Message.Length)
            {
                this.showChar = item.Message.Length;
            }
            this.messageText.text = item.Message.Substring(0, (int)this.showChar);
        }

        if(Input.GetButtonDown(nextButton))
        {
            this.NextMessage();
        }
    }

    /// <summary>
    /// ファイルをロードします。
    /// </summary>
    /// <param name="fileName">メッセージが記載されたファイル名</param>
    public void Load(string fileName)
    {
        this.Items.Clear();
        CSVLoader.CSV csv = CSVLoader.LoadFromResources(fileName);
        if(csv.GetRowCount() > 0)
        {
            ColumnName[] tags = new ColumnName[csv.GetColumnCount()];
            for(int column = 0; column < csv.GetColumnCount(); ++column)
            {
                switch(csv[0, column])
                {
                case "key": tags[column] = ColumnName.Key; break;
                case "name": tags[column] = ColumnName.Name; break;
                case "message": tags[column] = ColumnName.Message; break;
                case "memo": tags[column] = ColumnName.Memo; break;
                default:
                    Debug.LogError($"{csv[0, column]} is invalid tag. column = {column}.");
                    tags[column] = ColumnName.Invalid;
                    break;
                }
            }
            string key = string.Empty;
            string name = string.Empty;
            string message = string.Empty;
            for(int row = 1; row < csv.GetRowCount(); ++row)
            {
                for(int column = 0; column < csv.GetColumnCount(); ++column)
                {
                    switch(tags[column])
                    {
                    case ColumnName.Key: key = csv[row, column]; break;
                    case ColumnName.Name: name = csv[row, column]; break;
                    case ColumnName.Message: message = csv[row, column]; break;
                    }
                }
                this.Items.Add(new Message.Item(key, name, message));
            }
        }
        else
        {
            Debug.LogWarning($"{fileName} has not text.");
        }
    }

    /// <summary>
    /// メッセージを表示します。
    /// </summary>
    /// <param name="key">表示するメッセージのキー</param>
    public void StartMessage(string key)
    {
        this.Key = key;
        this.ItemIndex = -1;
        this.NextMessage();
    }

    /// <summary>
    /// メッセージを消去する
    /// </summary>
    public void ResetMessage()
    {
        this.messageText.text = string.Empty;
        this.nameText.text = string.Empty;
        this.UpdateVisibleWindow();
    }

    /// <summary>
    /// 次のメッセージまで進む
    /// なければ終了
    /// </summary>
    public void NextMessage()
    {
        this.ItemIndex++;
        var item = CurrentItem();
        if(item == null)
        {
            ResetMessage();
            return;
        }
        this.nameText.text = item.Name;
        this.UpdateVisibleWindow();
        this.showChar = 0;
    }

    /// <summary>
    /// 現在のメッセージアイテムを取得します。
    /// </summary>
    /// <returns></returns>
    private Item CurrentItem()
    {
        var keyItems = this.Items.FindAll(i => i.Key == this.Key);
        if(this.ItemIndex >= keyItems.Count)
        {
            return null;
        }
        return keyItems[this.ItemIndex];
    }

    /// <summary>
    /// ウィンドウの表示状態を更新する
    /// </summary>
    private void UpdateVisibleWindow()
    {
        this.messageWindow.SetActive(this.messageText.text != string.Empty);
        this.nameWindow.SetActive(this.nameText.text != string.Empty);
    }
}
