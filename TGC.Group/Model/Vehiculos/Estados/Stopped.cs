﻿using TGC.Core.Mathematica;
using TGC.Core.Sound;

namespace TGC.Group.Model.Vehiculos.Estados
{
    class Stopped : EstadoVehiculo
    {

        public Stopped(Vehicle auto) : base(auto)
        {
            auto.GetDeltaTiempoAvance().resetear();
        }

        public override TGCVector3 GetCarDirection()
        {
            //hay que probar si esto anda o produce efectos no esperados
            return new TGCVector3(0, 1, 0);
        }

        override public void Advance()
        {
            base.Advance();
            this.Move(auto.GetVectorAdelante() * auto.GetVelocidadActual() * auto.GetElapsedTime());
            this.auto.SetEstado(new Forward(this.auto));
        }

        override public void Back()
        {
            base.Back();
            this.Move(auto.GetVectorAdelante() * auto.GetVelocidadActual() * auto.GetElapsedTime());
            this.auto.SetEstado(new Backward(this.auto));
        }

        override public float Left()
        {
            float rotacionReal = -1f * auto.GetElapsedTime();
            auto.RotarDelanteras(rotacionReal);
            return 0;
        }

        override public float Right()
        {
            float rotacionReal = 1f * auto.GetElapsedTime();
            auto.RotarDelanteras(rotacionReal);
            return 0;
        }

        public override void UpdateWheels()
        {
            return;
        }
    }
}
