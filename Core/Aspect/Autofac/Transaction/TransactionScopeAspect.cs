using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Core.Utilities.Interceptors;
using Castle.DynamicProxy;

namespace Core.Aspects.Autofac.Transaction
{
    public class TransactionScopeAspect : MethodInterception
    {
        public override void Intercept(IInvocation invocation)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    invocation.Proceed();
                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                }
            }
        }
    }
}
