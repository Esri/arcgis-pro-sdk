/*

   Copyright 2018 Esri

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

   See the License for the specific language governing permissions and
   limitations under the License.

*/
using ArcGIS.Desktop.Framework;
using System.Windows.Input;

internal class AcmeBackstageVM : ArcGIS.Desktop.Framework.Contracts.BackstageTab
{
  private string _message;
  private RelayCommand _testCommand;

  public AcmeBackstageVM()
  {
    _testCommand = new RelayCommand(() => DoSomething(), false);
  }

  public ICommand TestCommand { get { return _testCommand; } }

  public string Message
  {
    get { return _message; }
    set
    {
      SetProperty(ref _message, value, () => Message);
    }
  }

  private void DoSomething()
  {
    Message = "Hello World!";
  }
}
