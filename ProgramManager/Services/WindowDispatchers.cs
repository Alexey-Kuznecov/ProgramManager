using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramManager.Views;

namespace ProgramManager.Services
{
    class WindowDispatchers : IDisposable
    {
        public IconsEditor IconsEditor;

        #region IDisposable Support

        private bool _disposedValue; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                    
                }
                
                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                _disposedValue = true;
               // IconsEditor.Close();
            }
        }
        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        ~WindowDispatchers()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
