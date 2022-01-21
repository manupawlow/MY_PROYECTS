using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drawings.Drawings
{
    public abstract class Draw : Form
    {
        int FRAME_RATE;
        int INIT_WIDTH;
        int INIT_HEIGHT;

        public abstract void MainLogic();
    }
}
