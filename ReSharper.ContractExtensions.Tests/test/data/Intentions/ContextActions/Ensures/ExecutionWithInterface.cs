using System.Diagnostics.Contracts;
using System;

[ContractClass(typeof(IAContract))]
interface IA
{
  strin{caret}g EnabledOnMethodWithContract();
}

[ContractClassFor(typeof(IA))]
abstract class IAContract : IA
{
  public string EnabledOnMethodWithContract()
  {
    throw new System.NotImplementedException();
  }
}