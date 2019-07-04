
# ObjectVisualization

## これは何ですか？

LINQPad の機能（変数の内容をグラフィカルに出力表示する機能）を、Visual Studio でデバッグ中に使いたいと思い、作成したライブラリです。
LINQPad 自体の機能は利用していないため、LINQPad が未インストールでも動作します。

業務コードや OSS 開発など、すでにあるソースを理解したいとか、変数の変化を時系列に追跡して、バグ調査したいなどの場面で利用できます。



## サンプルイメージ

![サンプルその１](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image01.png)


![サンプルその２](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image02.png)


![サンプルその３](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image03.png)


![サンプルその４](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image04.png)


![サンプルその５](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image05.png)


コールツリーやクラスの継承ツリーも表示できます。

![サンプルその６](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image06.png)




## 使い方

### その１、静的参照（dll 参照追加）

プロジェクトファイル（xxx.vbproj, xxx.csproj）に変更点が入っても大丈夫だよ！後で消すよ！ができる場合は、参照の追加から本ライブラリを追加します。
ただし、調査用のコードは書く必要があり、使い終わったらそれを削除するのを忘れないようにしてください。


#### C#

    using ObjectVisualization;

しておいて、

    xxx.Dump();

または、

    ObjectWatcher.Instance.Show();
    ObjectWatcher.Instance.Dump(xxx);

※基本的には、業務プログラム終了に任せて、Close メソッドを呼び出さなくてもいいです。

    ObjectWatcher.Instance.Close();

#### VB.NET

    Imports ObjectVisualization

しておいて、

    xxx.Dump()

または、

    ObjectWatcher.Instance.Show(TargetLanguageTypes.VBNET)
    ObjectWatcher.Instance.Dump(xxx)

Show メソッドの引数で、出力形式を VB.NET に設定します。

※基本的には、業務プログラム終了に任せて、Close メソッドを呼び出さなくてもいいです。

    ObjectWatcher.Instance.Close()

調査コードを書いたら、後はデバッグ実行します。ブレークポイントを設定しないで一気に実行してもいいですし、１行ずつステップ実行してもいいです。

拡張メソッドの Dump() は以下の型では使えません。この場合は ObjectWatcher.Instance.Dump(xxx) の方を使ってください。

| 項目 | 値                                                               |
| ----- |:---------------------------------------------------- |
| C# | dynamic 型                                                       |
| VB.NET | Object 型 |



### その２、動的参照（リフレクション利用）

プロジェクトファイルに変更点を加えたくない場合は、動的参照して使います。
ただし、やはり調査用のコードは書く必要があり、使い終わったらそれを削除するのを忘れないようにしてください。

以下は、リフレクション処理をラップしたメソッドとそれを使う側のサンプルです。このサンプルは違いますが、シングルトンクラスにすることでどこからでもアクセスできるので、楽に使えるかもしれません。

#### C# + .NET Framework 3.5 版

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    static void Dump(object dumpInstance)
    {
        // 動的に使いたい場合
        var dllFile = "ObjectVisualization.dll";
        if (!File.Exists(dllFile))
        {
            Console.WriteLine($"not found dll: ->\r\n{dllFile}");
            return;
        }

        // dll の参照追加したようなもの
        var asm = Assembly.LoadFrom(dllFile);

        // ObjectWatcher.Instance で生成されたインスタンスを取得したようなもの
        var classType = asm.GetType("ObjectVisualization.ObjectWatcher");
        var instanceType = classType.GetProperty("Instance");
        var instance = instanceType.GetValue(null, null);

        // Show() メソッドを呼び出したようなもの
        //var langEnumType = asm.GetType("ObjectVisualization.LanguageTypes");
        //var enumCSharp = 0;
        //var enumVBNET = 1;
        //var enumInstance = Convert.ChangeType(enumCSharp, Enum.GetUnderlyingType(langEnumType));

        var showMethod = classType.GetMethod("Show");
        //showMethod.Invoke(instance, new object[] { enumInstance });
        showMethod.Invoke(instance, new object[] { null });

        // Dump() メソッドを呼び出したようなもの
        var dumpMethod = classType.GetMethod("Dump");
        dumpMethod.Invoke(instance, new object[] { dumpInstance, new System.Diagnostics.StackFrame(1, true) });

        // Close() メソッドを呼び出したようなもの
        //Console.ReadKey();
        //var closeMethod = classType.GetMethod("Close");
        //closeMethod.Invoke(instance, null);
    }

    static void Main(string[] args)
    {
        Dump("hello, world");

        var items = new List<KeyValuePair<string, int>>();
        items.Add(new KeyValuePair<string, int>("aaa", 10));
        items.Add(new KeyValuePair<string, int>("bbb", 20));
        items.Add(new KeyValuePair<string, int>("ccc", 30));
        Dump(items);

        Console.ReadKey();
        Console.WriteLine("");
    }



#### C# + .NET Framework 4.7.2 版

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    static void Dump(object dumpInstance)
    {
        // 動的に使いたい場合
        var dllFile = "ObjectVisualization.dll";
        if (!File.Exists(dllFile))
        {
            Console.WriteLine($"not found dll: ->\r\n{dllFile}");
            return;
        }

        // dll の参照追加したようなもの
        var asm = Assembly.LoadFrom(dllFile);

        // ObjectWatcher.Instance で生成されたインスタンスを取得したようなもの
        var classType = asm.GetType("ObjectVisualization.ObjectWatcher");
        var instanceType = classType.GetProperty("Instance");
        var instance = instanceType.GetValue(null, null);

        // Show() メソッドを呼び出したようなもの
        //var langEnumType = asm.GetType("ObjectVisualization.LanguageTypes");
        //var enumCSharp = 0;
        //var enumVBNET = 1;
        //var enumInstance = Convert.ChangeType(enumCSharp, Enum.GetUnderlyingType(langEnumType));

        var showMethod = classType.GetMethod("Show");
        //showMethod.Invoke(instance, new object[] { enumInstance });
        showMethod.Invoke(instance, new object[] { null });

        // Dump() メソッドを呼び出したようなもの
        var callerInfo = new System.Diagnostics.StackFrame(1, true);
        var sourceFilePath = callerInfo.GetFileName();
        var memberName = callerInfo.GetMethod().Name;
        var sourceLineNumber = callerInfo.GetFileLineNumber();

        var dumpMethod = classType.GetMethod("Dump");
        dumpMethod.Invoke(instance, new object[] { dumpInstance, sourceFilePath, memberName, sourceLineNumber });

        // Close() メソッドを呼び出したようなもの
        //Console.ReadKey();
        //var closeMethod = classType.GetMethod("Close");
        //closeMethod.Invoke(instance, null);
    }

    static void Main(string[] args)
    {
        Dump("hello, world");

        var items = new List<KeyValuePair<string, int>>();
        items.Add(new KeyValuePair<string, int>("aaa", 10));
        items.Add(new KeyValuePair<string, int>("bbb", 20));
        items.Add(new KeyValuePair<string, int>("ccc", 30));
        Dump(items);

        Console.ReadKey();
        Console.WriteLine("");
    }


#### VB.NET + .NET Framework 3.5 版

    Imports System.IO
    Imports System.Reflection

    Sub Dump(ByVal dumpInstance As Object)

        ' 動的に使いたい場合
        Dim dllFile As String = "ObjectVisualization.dll"
        If Not File.Exists(dllFile) Then
            Console.WriteLine($"not found dll: ->{vbNewLine}{dllFile}")
            Return
        End If

        ' dll の参照追加したようなもの
        Dim asm As Assembly = Assembly.LoadFrom(dllFile)

        ' ObjectWatcher.Instance で生成されたインスタンスを取得したようなもの
        Dim classType As Type = asm.GetType("ObjectVisualization.ObjectWatcher")
        Dim instanceType As PropertyInfo = classType.GetProperty("Instance")
        Dim instance As Object = instanceType.GetValue(Nothing, Nothing)

        ' Show() メソッドを呼び出したようなもの
        Dim langEnumType As Type = asm.GetType("ObjectVisualization.LanguageTypes")
        Dim enumCSharp As Integer = 0
        Dim enumVBNET As Integer = 1
        Dim enumInstance As Object = Convert.ChangeType(enumVBNET, [Enum].GetUnderlyingType(langEnumType))

        Dim showMethod As MethodInfo = classType.GetMethod("Show")
        showMethod.Invoke(instance, New Object() {enumInstance})

        ' Dump() メソッドを呼び出したようなもの
        Dim dumpMethod = classType.GetMethod("Dump")
        dumpMethod.Invoke(instance, New Object() {dumpInstance, New StackFrame(1, True)})

        ' Close() メソッドを呼び出したようなもの
        'Console.ReadKey()
        'Dim closeMethod As MethodInfo = classType.GetMethod("Close")
        'closeMethod.Invoke(instance, Nothing)

    End Sub

    Sub Main()

        Dump("hello, world")

        Dim items As New List(Of KeyValuePair(Of String, Integer))
        items.Add(New KeyValuePair(Of String, Integer)("aaa", 10))
        items.Add(New KeyValuePair(Of String, Integer)("bbb", 20))
        items.Add(New KeyValuePair(Of String, Integer)("ccc", 30))
        Dump(items)

        Console.ReadKey()
        Console.WriteLine("")

    End Sub



#### VB.NET + .NET Framework 4.7.2 版

    Imports System.IO
    Imports System.Reflection

    Sub Dump(ByVal dumpInstance As Object)

        ' 動的に使いたい場合
        Dim dllFile As String = "ObjectVisualization.dll"
        If Not File.Exists(dllFile) Then
            Console.WriteLine($"not found dll: ->{vbNewLine}{dllFile}")
            Return
        End If

        ' dll の参照追加したようなもの
        Dim asm As Assembly = Assembly.LoadFrom(dllFile)

        ' ObjectWatcher.Instance で生成されたインスタンスを取得したようなもの
        Dim classType As Type = asm.GetType("ObjectVisualization.ObjectWatcher")
        Dim instanceType As PropertyInfo = classType.GetProperty("Instance")
        Dim instance As Object = instanceType.GetValue(Nothing, Nothing)

        ' Show() メソッドを呼び出したようなもの
        Dim langEnumType As Type = asm.GetType("ObjectVisualization.LanguageTypes")
        Dim enumCSharp As Integer = 0
        Dim enumVBNET As Integer = 1
        Dim enumInstance As Object = Convert.ChangeType(enumVBNET, [Enum].GetUnderlyingType(langEnumType))

        Dim showMethod As MethodInfo = classType.GetMethod("Show")
        showMethod.Invoke(instance, New Object() {enumInstance})

        ' Dump() メソッドを呼び出したようなもの
        Dim callerInfo As New StackFrame(1, True)
        Dim sourceFilePath As String = callerInfo.GetFileName()
        Dim memberName As String = callerInfo.GetMethod().Name
        Dim sourceLineNumber As Integer = callerInfo.GetFileLineNumber()

        Dim dumpMethod = classType.GetMethod("Dump")
        dumpMethod.Invoke(instance, New Object() {dumpInstance, sourceFilePath, memberName, sourceLineNumber})

        ' Close() メソッドを呼び出したようなもの
        'Console.ReadKey()
        'Dim closeMethod As MethodInfo = classType.GetMethod("Close")
        'closeMethod.Invoke(instance, Nothing)

    End Sub

    Sub Main()

        Dump("hello, world")

        Dim items As New List(Of KeyValuePair(Of String, Integer))
        items.Add(New KeyValuePair(Of String, Integer)("aaa", 10))
        items.Add(New KeyValuePair(Of String, Integer)("bbb", 20))
        items.Add(New KeyValuePair(Of String, Integer)("ccc", 30))
        Dump(items)

        Console.ReadKey()
        Console.WriteLine("")

    End Sub




調査コードを書いたら、後はデバッグ実行します。ブレークポイントを設定しないで一気に実行してもいいですし、１行ずつステップ実行してもいいです。



## ダウンロード

[リリースページへ移動](https://github.com/sutefu7/ObjectVisualization/releases)

本プログラムは、以下の環境をターゲットにしています。

| 項目 | 値                                                               |
| ----- |:---------------------------------------------------- |
| ターゲット言語 | C# / VB.NET                                                       |
| ターゲットフレームワーク | .NET Framework 3.5 版 / 4.7.2 版 |


それ以外が欲しい方は、プロジェクト一式をダウンロードしてきて、ターゲットフレームワークを切り替えてリビルドすると作成できます。もしくは空のプロジェクトを作成して、ソースファイルを追加するなどです。



## 開発環境

| 項目 | 値                                                               |
| ----- |:---------------------------------------------------- |
| OS   | Windows 10 Pro (64 bit)                              |
| IDE  | Visual Studio Community 2017                     |
| 言語 | C#                                                       |
| 種類 | クラスライブラリ (.NET Framework 3.5 版 / 4.7.2 版) |



## 設計思想

完コピは目的ではないため、未実装の機能があったりします。また、一部の表現方法に差異があります。こっちの方が分かりやすいと思ったからとか、技術的に実現不可だったからとか、そういう理由からです。ただ、より良くするために将来直すかもです。

バグ指摘・要望、等々を受け付けています。ご連絡をお待ちしております。この時、もしも可能でしたらサンプルコードなどのお力添えがあると嬉しいです。



