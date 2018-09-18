Option Strict On
Option Explicit On

Namespace Helpers
    Module Sha1Generator
        Public Function Generate(ByVal strToHash As String) As String
            Dim sha1Obj As New Security.Cryptography.SHA1CryptoServiceProvider
            Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes( _
                strToHash _
                )

            bytesToHash = sha1Obj.ComputeHash(bytesToHash)

            Dim strResult As String = ""

            For Each b As Byte In bytesToHash
                strResult += b.ToString("x2")
            Next

            Return strResult
        End Function
    End Module
End Namespace
