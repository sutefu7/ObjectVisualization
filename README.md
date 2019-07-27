
# ObjectVisualization

## これは何ですか？

デバッグ中の変数を視覚化して見るためのライブラリです。Visual Studio のデバッガビジュアライザーとして作成していますので、ソースコードへの変更無しに、いつも通りのデバッグ作業として使うことができます。

使い方は簡単で、dll をマイドキュメントにある Visual Studio の指定フォルダに配置するだけです。消し方も簡単で、配置した dll を削除するだけです。

## デバッガビジュアライザーって何？

例えば、Visual Studio 2017 だと、テキスト、XML、HTML、DataSet 向けのビジュアライザーなどが標準搭載されています。DataSet 向けだとこういうのです。

![サンプルその1](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image01.png)

例えば DataTable はウォッチウィンドウでツリー展開して見るよりも、表形式で見たほうが分かりやすいです。こんな便利なビジュアライザーをもっと多くの型で使えたらいいのに。という困り事から作成しようと思いました。

## 作成したビジュアライザー

### コレクション系

DataRow, DataRowView, List&lt;T&gt; などのコレクション系の型に対して、表形式で見られるビジュアライザーを用意しました。

![サンプルその2](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image02.png)

ただし、ビジュアライザー側の仕様的に、インターフェース型は未対応っぽくて、クラス別になるため、現状未対応のコレクション系もあるかもしれませんが、随時対応していきたいと考えています。

また、表形式（２次元配列）で表示するのですが、特に Entity Framework の DbSet&lt;T&gt; などでは int や string などのプリミティブ型以外に、リレーションとしてデータクラスを持っていると思います。この場合、そのインスタンスの ToString() した名前空間が表示されるという制限になります。

![サンプルその3](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image03.png)

このサンプル画像で出ていますが、Entity Framework は解決できないバグがあるため未対応です。よって、DbContext は未対応、DbSet&lt;T&gt; は ToList() に変換したものでビジュアライザーで見ていただく使い方になります。この制限は IEnumerable&lt;T&gt; 系も同様です。

### XML 操作系

XmlDocument, XmlElement, XDocument, XElement は、構造とブラウザ表示の２部構成のビジュアライザーを用意しました。

![サンプルその4](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image04.png)

### その他全部の型

これまで紹介してきた各ビジュアライザーとは別に、全ての型に対応するビジュアライザーを用意しました。もちろんインターフェース型にも対応しています。

![サンプルその5](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image05.png)

![サンプルその6](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image06.png)

ただし、object 型に対応させることができないため、代わりに WeakReference 型を指定しています。使うときはウォッチウィンドウなどから `new WeakReference(xxx)` として、WeakReference 型に変換することでビジュアライザーに渡すことができます。

前述の XML 系のビジュアライザーもそうですが、変数の表示形式を LINQPad に似せています。ツリー形式の文字列として見るよりも、視覚化して見たほうがはるかに分かりやすいからです。

ただし、制限としてコレクション系データは先頭 10 件のみ表示しています。理由は、実装上の仕組みの話で、ビジュアライザーに渡す際、データ量が多くなりすぎるとタイムアウトで渡しきれなくなるためです。

#### 時系列の履歴管理

現在表示中のデータを、次回表示時に再度表示させることができます。関連する変数同士を見たいときに使います。

![サンプルその7](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image07.png)

チェック欄にチェックを付けたら、いったんビジュアライザー画面を閉じます。続けて、任意のタイミングでビジュアライザーを表示させます。

![サンプルその8](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image08.png)

この機能を使うと、dll があるフォルダ内に作業用フォルダが作成され、この中にテキストファイルとしてデータを保存したり削除したりします。

- マイドキュメント
	- Visual Studio 201X
		- Visualizers
			- ObjectVisualization.dll
			- ObjectVisualization_Work
				- ObjectInfo
					- yyyy_MM_dd_HH_mm_ss_fff.txt　←★これ


#### 差分チェック

前述の `時系列の履歴管理` を応用して、同じ型でかつ同じデータ数の場合だけなのですが、差分チェック機能を搭載しています。想定場面としては、同じ変数でも、ある行を通る前の変数、通った後の変数の見比べです。

![サンプルその9](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image09.png)

#### 継承元クラスツリー

`new WeakReference(xxx.GetType())` みたいに Type 型を渡すことで、継承関係を見ることができます。

![サンプルその10](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image10.png)

#### コールツリー

ステップ実行中の行位置をベースとして、コールツリーを見ることができます。

やり方は、ウォッチウィンドウなどで `new WeakReference(new System.Diagnostics.StackTrace(true).GetFrames())` と入力します。

![サンプルその11](https://raw.githubusercontent.com/sutefu7/ObjectVisualization/master/docs/images/image11.png)

## 旧仕様

以前提供していた、ソースコード上からデータ表示する専用命令（`ObjectWatcher.Instance.Dump()`, `xxx.Dump()`）は削除しました。


## インストール

マイドキュメントにある Visual Studio の Visualizers フォルダ（例えば私の場合、`C:\Users\Account\Documents\Visual Studio 2017\Visualizers`） に、dll を配置します。それだけです。

## アンインストール

上記に配置した dll を削除します。

## ダウンロード

[リリースページへ移動](https://github.com/sutefu7/ObjectVisualization/releases)

本プログラムは、以下の環境をターゲットにしています。

| 項目 | 値                                                               |
| ----- |:---------------------------------------------------- |
| ターゲット言語 | C# / VB.NET                                                       |
| ターゲットフレームワーク | .NET Framework 3.5 版 / 4.X 版 |

ソースコードは、.NET Framework 3.5 版 と 4.0 版をメンテナンスしています。Visual Studio の仕様的に、開いているソースコードが .NET Framework 3.5 の場合は、.NET Framework 3.5 版のライブラリを、.NET Framework 4.X の場合は、.NET Framework 4.0 版のライブラリを配置します。つまり、使うソースコードの .NET Framework バージョンによって、ライブラリを切り替える必要があります。同名で名前空間なども同一のため、両方配置は無理なはずです。

## 開発環境

| 項目 | 値                                                               |
| ----- |:---------------------------------------------------- |
| OS   | Windows 10 Pro (64 bit)                              |
| IDE  | Visual Studio Community 2017                     |
| 言語 | C#                                                       |
| 種類 | クラスライブラリ (.NET Framework 3.5 版 / 4.0 版) |


