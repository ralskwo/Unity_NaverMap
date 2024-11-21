#if CSHARP_7_3_OR_NEWER

namespace Gpm.Common.ThirdParty.SharpCompress.Compressors.Rar.UnpackV2017
{

internal partial class BitInput
{
    public const int MAX_SIZE=0x8000; // Size of input buffer.

    public int InAddr; // Curent byte position in the buffer.
    public int InBit;  // Current bit position in the current byte.

    public bool ExternalBuffer;

    //BitInput(bool AllocBuffer);
    //~BitInput();

    public byte[] InBuf; // Dynamically allocated input buffer.

    public
    void InitBitInput()
    {
      InAddr=InBit=0;
    }
    
    // Move forward by 'Bits' bits.
    public void addbits(uint _Bits)
    {
      var Bits = checked((int)_Bits);
      Bits+=InBit;
      InAddr+=Bits>>3;
      InBit=Bits&7;
    }
    
    // Return 16 bits from current position in the buffer.
    // Bit at (InAddr,InBit) has the highest position in returning data.
    public uint getbits()
    {
      uint BitField=(uint)InBuf[InAddr] << 16;
      BitField|=(uint)InBuf[InAddr+1] << 8;
      BitField|=(uint)InBuf[InAddr+2];
      BitField >>= (8-InBit);
      return BitField & 0xffff;
    }

    // Return 32 bits from current position in the buffer.
    // Bit at (InAddr,InBit) has the highest position in returning data.
    public uint getbits32()
    {
      uint BitField=(uint)InBuf[InAddr] << 24;
      BitField|=(uint)InBuf[InAddr+1] << 16;
      BitField|=(uint)InBuf[InAddr+2] << 8;
      BitField|=(uint)InBuf[InAddr+3];
      BitField <<= InBit;
      BitField|=(uint)InBuf[InAddr+4] >> (8-InBit);
      return BitField & 0xffffffff;
    }
    
    //void faddbits(uint Bits);
    //uint fgetbits();
    
    // Check if buffer has enough space for IncPtr bytes. Returns 'true'
    // if buffer will be overflown.
  private bool Overflow(uint IncPtr) 
    {
      return InAddr+IncPtr>=MAX_SIZE;
    }

    //void SetExternalBuffer(byte *Buf);
}

}


#endif