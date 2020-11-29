﻿using System;
using System.Reflection;
using System.Text;

namespace SessionServer.Sessions {
    public class CPacket : IDisposable {
        public const int PACKET_BEGIN = 0x8F;
        public const int HEADER_SIZE = 3;
        public const int MESSAGE_BUFFER_SIZE = 1024;

        public byte[] Buffer { get; internal set; }
        public int Position { get; internal set; } = HEADER_SIZE;

        /// <summary>
        /// CPacket 에 아무것도 들어있지 않은지 
        /// </summary>
        public bool IsEmpty {
            get {
                return Position <= HEADER_SIZE;
            }
        } 

        public static CPacket NewSend {
            get {
                return new CPacket();
            }
        }
        public static CPacket NewReceive {
            get {
                return new CPacket() {
                    Position = 0,
                };
            }
        }


        public CPacket() {
            Buffer = new byte[1024];
        }
    
        ~CPacket() {
            Recycle();
        }

        public void Recycle() {
            if (Buffer != null) {
                Buffer = null;
            }
        }

        public void Sealed() {
            var b = Buffer;
            var dataLength = Position - HEADER_SIZE;

            b[0] = PACKET_BEGIN;
            b[1] = (byte)(dataLength);
            b[2] = (byte)(dataLength >> 8);
        }

        public void MoveToFirst() {
            this.Position = HEADER_SIZE;
        }

        void IDisposable.Dispose() {
            Recycle();
        }
    }
}
