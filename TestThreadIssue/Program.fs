// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.Windows

open FsXaml
open Gjallarhorn
open Gjallarhorn.Bindable
open Gjallarhorn.Wpf

type View = XAML<"View.xaml">

[<STAThread; EntryPoint>]
let main _ = 
    Platform.install true |> ignore

    let bindingSource = Binding.createObservableSource ()

    let canExecute = Mutable.create true

    let command =
        bindingSource
        |> Binding.createCommandChecked "TestCommand" canExecute

    let mappedCommand = 
        command
        |> Observable.mapAsync (fun _ -> async { return () })

    mappedCommand
    |> Observable.subscribe (fun _ -> 
        canExecute.Value <- false
        ())
    |> bindingSource.AddDisposable
    
    let app = Application ()
    let view = View ()

    view.DataContext <- bindingSource

    app.Run view |> ignore
    0 // return an integer exit code
