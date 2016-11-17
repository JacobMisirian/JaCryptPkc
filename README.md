#JaCryptPkc
Public Key Cryptography Implementation of JaCrypt (https://github.com/JacobMisirian/JaCrypt)

##How Public Key Cryptography Works

Public Key Cryptography (or Pkc) is a cryptographic method that allows people to share
an encryption key with others, allowing them to encrypt their data using that key. Only
the owner of that public key can decrypt the data using their private key, which is
mathematically related to the public key.

The data itself is encrypted using a traditional Symmetric Key Encryption algorithm, where
both parties share a common key used to encrypt and decrypt data. In Public Key Cryptography,
this key is randomly generated, and the public key is used to encrypt the symmetric key for
the data.

Suppose Alice wants to send a message to Bob. Alice begins by encrypting her message using
a random key. The key is then encrypted using Bob's public key and included in the
message. When Bob receives the message, he uses his private key to decrypt the key for
the data, finally using this key to decrypt the actual data. Any thrid party listener who
intercepted Alice's message would be unable to decrypt the key, and therefore unable
to decrypt the data.

##How JaCryptPkc Works

###Message Encryption

In JaCryptPkc, the message is first encrypted using the JaCrypt encryption algorithm with
a random 1024 bit key. This key is then encrypted using the RSA trapdoor function using
the public key of the intended recipient. The encrypted key and encrypted message are
concatonated together and returned.

###Message Decryption

When the receiver gets the encrypted message, the RSA trapdoor function is used to decrypt
the key for the data using the private (and public) keys. From there, the message can be
decrypted using the JaCrypt Encryption algorithm with the decrypted key.

###Key Generation

In any Public Key Cryptography algorithm, a public and private key are employed to secure
the data. The algorithm is still responsible for generating these keys, which are inherently
linked with the RSA trapdoor function.

The public and private keys are generated from two random prime numbers, named ```p``` and ```q```.
JaCryptPkc allows you to supply these numbers yourself or generate a random prime number. It is recommended
that only primes greater than 2^512 be used for ```p``` and ```q```, as the random symmetric key
must be less than ```p * q``` in order to function properly. ```p``` and ```q``` are used to 
generate a public key and private key. The public key is what you want to give out. Anyone should
have access to the key so they can encrypt their data using it. The private key is something the
keep secret, so that only you can decrypt data encrypted with your public key.
