﻿using TGC.Core.Mathematica;
using TGC.Core.Sound;

namespace TGC.Group.Model.Vehiculos.Estados
{
    class Forward : EstadoVehiculo
    {

        public Forward(Vehicle auto) : base(auto)
        {
        }

        public override TGCVector3 GetCarDirection()
        {
            return this.auto.GetVectorAdelante();
        }

        public override void Advance()
        {
            base.Advance();
            Move(auto.GetVectorAdelante() * auto.GetVelocidadActual() * auto.GetElapsedTime());
        }

        public override void Back()
        {
            auto.breakLights.ActivateLight();
            Brake(auto.GetConstanteFrenado());
        }

        public override void SpeedUpdate()
        {
            Brake(auto.GetConstanteRozamiento());
        }

        private void Brake(float constante)
        {
            
            auto.GetDeltaTiempoAvance().resetear();
            auto.SetVelocidadActual(auto.GetVelocidadActual() - constante);
            if (auto.GetVelocidadActual() < 0)
            {
                auto.SetVelocidadActual(0);
                auto.GetDeltaTiempoAvance().resetear();
                this.auto.SetEstado(new Stopped(this.auto));

                return;
            }
            this.Move(auto.GetVectorAdelante() * auto.GetVelocidadActual() * auto.GetElapsedTime());
        }

    }
}
