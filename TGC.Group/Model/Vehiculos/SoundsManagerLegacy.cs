﻿using Microsoft.DirectX.DirectSound;
using System;
using TGC.Core.Mathematica;
using TGC.Group.MyAbstractions;

namespace TGC.Group.Model.Vehiculos
{
    class SoundsManagerLegacy
    {
        /// <summary>
        ///     acceleratingWAV y desacceleratingWAV deben ser el mismo audio pero opuesto
        ///     maxVelWAV y minVelWAV tienen que coincidir con el principio y el final de acceleratingWAV
        /// </summary>
        Tgc3dSound minVelSound;
        Tgc3dSound maxVelSound;
        Tgc3dSound acceleratingVelSound;
        Tgc3dSound desacceleratingVelSound;
        int LAST_BUFFER_POSITION = 702072;
        public SoundsManagerLegacy(String minVelWAV, String maxVelWAV, String acceleratingWAV, String desacceleratingWAV, TGCVector3 position)
        {
            var bufferDescription = new BufferDescription();
            bufferDescription.ControlFrequency = true;
            this.minVelSound = new Tgc3dSound(minVelWAV, position, GlobalConcepts.GetInstance().GetDispositivoDeAudio());
            this.maxVelSound = new Tgc3dSound(maxVelWAV, position, GlobalConcepts.GetInstance().GetDispositivoDeAudio());
            this.acceleratingVelSound = new Tgc3dSound(acceleratingWAV, position, GlobalConcepts.GetInstance().GetDispositivoDeAudio(), bufferDescription);
            this.desacceleratingVelSound = new Tgc3dSound(desacceleratingWAV, position, GlobalConcepts.GetInstance().GetDispositivoDeAudio(), bufferDescription);
            this.desacceleratingVelSound.SoundBuffer.SetCurrentPosition(LAST_BUFFER_POSITION);
            this.PlayMinVel();
        }

        public void PlayMinVel()
        {
            if (this.minVelSound.SoundBuffer.Status.Playing) return;
            this.desacceleratingVelSound.stop();
            this.maxVelSound.stop();
            this.desacceleratingVelSound.SoundBuffer.SetCurrentPosition(LAST_BUFFER_POSITION);
            this.acceleratingVelSound.SoundBuffer.SetCurrentPosition(0);
            this.minVelSound.play(true);
        }

        public void PlayMaxVel()
        {
            if (this.maxVelSound.SoundBuffer.Status.Playing) return;
            this.acceleratingVelSound.stop();
            this.minVelSound.stop();
            this.acceleratingVelSound.SoundBuffer.SetCurrentPosition(LAST_BUFFER_POSITION);
            this.desacceleratingVelSound.SoundBuffer.SetCurrentPosition(0);
            this.maxVelSound.play(true);
        }

        public void PlayAccelerating()
        {
            this.desacceleratingVelSound.stop();
            this.minVelSound.stop();
            if (this.maxVelSound.SoundBuffer.Status.Playing) return;
            if (this.acceleratingVelSound.SoundBuffer.Status.Playing && this.acceleratingVelSound.SoundBuffer.PlayPosition > 700000)
            {
                this.acceleratingVelSound.stop();
                this.PlayMaxVel();
                return;
            }
            if (this.acceleratingVelSound.SoundBuffer.Status.Playing) return;
            this.maxVelSound.stop();
            var playFrom = this.desacceleratingVelSound.SoundBuffer.PlayPosition;
            this.acceleratingVelSound.SoundBuffer.SetCurrentPosition(this.desacceleratingVelSound.SoundBuffer.Caps.BufferBytes - playFrom);
            this.acceleratingVelSound.play();
        }

        public void PlayDesaccelerating()
        {
            this.acceleratingVelSound.stop();
            this.maxVelSound.stop();
            if (this.minVelSound.SoundBuffer.Status.Playing) return;
            if (this.desacceleratingVelSound.SoundBuffer.Status.Playing && this.desacceleratingVelSound.SoundBuffer.PlayPosition > 700000) {
                this.PlayMinVel();
                return;
            };
            if (this.desacceleratingVelSound.SoundBuffer.Status.Playing) return;
            this.minVelSound.stop();
            var playFrom = this.acceleratingVelSound.SoundBuffer.PlayPosition;
            // last cursor position 702072
            // this.desacceleratingVelSound.SoundBuffer.Caps.BufferBytes 705600
            // why difference?
            this.desacceleratingVelSound.SoundBuffer.SetCurrentPosition(LAST_BUFFER_POSITION - playFrom);
            this.desacceleratingVelSound.play();
        }

        public void SetAccFrequency(int freq)
        {
            this.acceleratingVelSound.SoundBuffer.Frequency = freq;
        }

        public void SetDesAccFrequency(int freq)
        {
            this.desacceleratingVelSound.SoundBuffer.Frequency = freq;
        }
    }
}
