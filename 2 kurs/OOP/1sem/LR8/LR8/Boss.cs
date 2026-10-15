using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR8
{
    public delegate void Turn(int voltage);
    public class Boss
    {
        public event Action Upgrade;
        public event Turn TurnOn;

        public void HaveUpgrade()
        {
            Upgrade?.Invoke();
        }

        public void HaveTurnOn(int voltage)
        {
            TurnOn?.Invoke(voltage);
        }
    }
}
