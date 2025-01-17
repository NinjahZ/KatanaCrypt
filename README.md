# KatanaCrypt

**KatanaCrypt** is a powerful file encryption and decryption utility built in C#. It utilizes the **Advanced Encryption Standard (AES)** algorithm with a **256-bit key** in combination with **PBKDF2 (Password-Based Key Derivation Function 2)** to ensure a high level of security for your sensitive data. Whether you're storing personal files or transmitting critical information, **KatanaCrypt** guarantees the protection of your data with industry-standard encryption techniques.

---

## ⚙️ Features

- **AES Encryption & Decryption**  
  Encrypt your files using **AES-256** with a secure key derived from a password. This encryption ensures that your files remain safe from unauthorized access.

- **PBKDF2 Key Derivation**  
  Strengthen your password-based encryption using the **PBKDF2** function, making it more resistant to brute-force attacks and preventing common cracking methods like dictionary or rainbow table attacks.

- **Metadata Protection**  
  Not only encrypts your file content, but also securely stores metadata (such as the original file name) using the same AES encryption for added confidentiality.

- **File Integrity Check (HMAC)**  
  Ensures the integrity of your file by using an **HMAC** (Hash-based Message Authentication Code), making sure the data hasn't been tampered with during the encryption or decryption process.

- **Ease of Use**  
  Simple graphical user interface (GUI) for easy file selection, encryption, and decryption with minimal user intervention.

- **Cross-Platform**  
  Although currently built for **Windows**, the principles and algorithms used are easily portable to other platforms.

---

## 🛠️ How It Works

### **Encryption Process**:

1. **Password Input**:  
   Users enter a password that is used to derive a cryptographic key through **PBKDF2**.
   
2. **Salt & IV Generation**:  
   A random **salt** and **initialization vector (IV)** are generated for each file to ensure unique encryption, even with identical files and passwords.

3. **File Encryption**:  
   The **AES** algorithm encrypts the file using the derived key, salt, and IV.

4. **Metadata Storage**:  
   The original file name is encrypted and stored in the output file along with the salt, IV, and **HMAC**, ensuring the integrity and confidentiality of the data.

---

### **Decryption Process**:

1. **Password Input**:  
   Users enter the password used during encryption.

2. **File Integrity Check**:  
   The **HMAC** is verified to confirm that the file has not been altered since encryption.

3. **Metadata Decryption**:  
   The original file name is decrypted from the metadata.

4. **File Decryption**:  
   Using the password-derived key, the **AES** algorithm decrypts the file back to its original state.

---

## 🔒 Security Considerations

- **AES-256**: The encryption utilizes **AES with a 256-bit key**, widely regarded as one of the most secure encryption algorithms available today.

- **PBKDF2**:  
  A key strengthening technique to defend against brute-force attacks, making it significantly harder to derive the encryption key from the password alone.

- **HMAC**:  
  Protects the data integrity of the file by ensuring that the data has not been tampered with during storage or transfer.

- **Random Salt & IV**:  
  Each file is encrypted with a unique salt and IV, guaranteeing that even identical files encrypted with the same password will have different ciphertexts.

---

## ⚡ Performance

KatanaCrypt has been optimized to provide a balance between **security** and **performance**. The encryption and decryption process can take longer with larger files, but the underlying **AES encryption** is highly efficient. Below are some performance benchmarks:

| **File Size** | **Encryption Time** | **Decryption Time** |
|---------------|---------------------|---------------------|
| 1 MB          | 0.1 seconds         | 0.09 seconds        |
| 10 MB         | 1 second            | 0.8 seconds         |
| 100 MB        | 10 seconds          | 8 seconds           |
| 1 GB          | 1 minute            | 45 seconds          |

> **Note**:  
> Performance can vary depending on hardware, including CPU and potential use of **hardware acceleration**.

---

## 💻 Installation

1. **Download**  
   You can download the latest release of **KatanaCrypt** from the [Releases page](https://github.com/NinjahZ/KatanaCrypt/releases).

2. **Run**  
   Once downloaded, simply run the `KatanaCrypt.exe` application. There's no installation required.

3. **Use**  
   Select the file you want to encrypt or decrypt, enter a password, and let the program handle the rest.

---

## 📝 How to Use

### **Encrypting a File**:

1. Click on the "Select File" button to choose the file you want to encrypt.
2. Enter a strong password in the password prompt.
3. Press the **"Encrypt"** button. The encrypted file will be saved with a `.kenc` extension.

---

### **Decrypting a File**:

1. Select an encrypted file (with a `.kenc` extension).
2. Enter the same password used to encrypt the file.
3. Press the **"Decrypt"** button. The original file will be restored with its original name.

---

## 🔐 Password Security

While **KatanaCrypt** offers robust security, the strength of your encrypted files ultimately depends on the password you choose. For maximum security, use a strong password:

- At least 12 characters long
- Include a mix of uppercase and lowercase letters, numbers, and symbols
- Avoid easily guessable information (e.g., names, birthdates)

---

## 📜 License

This project is licensed under the **GNU General Public License v3.0**.  
See the [LICENSE file](LICENSE) for details.

---

## 🤝 Contributing

We welcome contributions to **KatanaCrypt**! To contribute:

1. Fork the repository
2. Make your changes
3. Submit a pull request

---

## 🐞 Reporting Issues

If you encounter any issues or have suggestions for improvements, please open an issue in the [GitHub Issues section](https://github.com/NinjahZ/KatanaCrypt/issues).
