using System;
using System.Collections.Generic;

namespace CybersecurityChatbott
{
    public class KeywordResponder
    {
        private Dictionary<string, List<string>> _responses;
        private Random _random = new Random();

        public KeywordResponder()
        {
            _responses = new Dictionary<string, List<string>>
            {
                {
                    "password",
                    new List<string>
                    {
                        "Use strong passwords with symbols and numbers.",
                        "Avoid using your birthday as a password.",
                        "Change important passwords regularly."
                    }
                },
                {
                    "scam",
                    new List<string>
                    {
                        "Scam protection tips:\n" +
                        "- Be skeptical of offers that sound too good to be true.\n" +
                        "- Verify identities and never share payment details with unverified contacts.\n" +
                        "- Use official channels to confirm requests involving money or personal data."
                    }
                },
                {
                    "what is scam",
                    new List<string>
                    {
                        "Scam definition:\n" +
                        "A scam is a fraudulent scheme designed to trick people into giving money, information, or access. Scams can appear via email, phone, websites, or social media.\n" +
                        "Mitigation: verify requests, check URLs carefully, and report scams to authorities or platform providers."
                    }
                },

                {
                    "phishing",
                    new List<string>
                    {
                        // Detailed guidance for phishing
                        "Phishing prevention steps:\n" +
                        "1) Do not click links or open attachments from unknown or unexpected senders.\n" +
                        "2) Verify the sender by checking the email address (not just the display name) and contacting the organization through an independent channel.\n" +
                        "3) Look for signs of impersonation: typos, mismatched domains, urgent requests for sensitive data.\n" +
                        "4) Enable multi-factor authentication (MFA) on important accounts so stolen passwords alone can't be used.\n" +
                        "5) Keep software and your browser up to date and use security features like anti-phishing filters.\n" +
                        "6) If unsure, report the message to your IT/security team and delete it.",

                        "Quick tips: Hover over links to see the real URL before clicking, don't provide sensitive information in response to email prompts, and use a reputable password manager to avoid credential reuse."
                    }
                },
                {
                    "malware",
                    new List<string>
                    {
                        // Malware prevention and response
                        "Malware protection steps:\n" +
                        "1) Keep your operating system, applications and antivirus up to date.\n" +
                        "2) Install and maintain reputable anti-malware/antivirus software and run regular scans.\n" +
                        "3) Avoid downloading software from untrusted websites; prefer official stores and vendor sites.\n" +
                        "4) Be cautious with email attachments and links — many malware campaigns start with phishing.\n" +
                        "5) Use least-privilege accounts: don't run as administrator for everyday tasks.\n" +
                        "6) Regularly back up important data to an offline or cloud backup so you can recover if infected.\n" +
                        "7) If infected, disconnect from networks, run a full scan, and consult security professionals if needed."
                    }
                },
                {
                    "protect",
                    new List<string>
                    {
                        "General protection advice:\n" +
                        "- Use strong, unique passwords and a password manager.\n" +
                        "- Enable multi-factor authentication.\n" +
                        "- Keep devices and apps updated.\n" +
                        "- Be careful with links and attachments.\n" +
                        "- Regularly back up data and use reputable security software."
                    }
                },

                {
                    "wifi",
                    new List<string>
                    {
                        "Wi‑Fi safety tips:\n" +
                        "1) Use WPA3 or WPA2 encryption on your home router and change the default admin password.\n" +
                        "2) Keep router firmware updated.\n" +
                        "3) Avoid using public open Wi‑Fi for sensitive tasks; use a VPN when necessary.\n" +
                        "4) Disable WPS and guest networks when not needed."
                    }
                },
                {
                    "wi-fi",
                    new List<string>
                    {
                        "Wi‑Fi safety tips:\n" +
                        "1) Use WPA3 or WPA2 encryption on your home router and change the default admin password.\n" +
                        "2) Keep router firmware updated.\n" +
                        "3) Avoid using public open Wi‑Fi for sensitive tasks; use a VPN when necessary.\n" +
                        "4) Disable WPS and guest networks when not needed."
                    }
                },

                {
                    "vpn",
                    new List<string>
                    {
                        "VPN guidance:\n" +
                        "- Use a reputable VPN provider when on untrusted networks.\n" +
                        "- A VPN encrypts traffic between you and the VPN server, but it does not make you invulnerable; pick a trustworthy provider.\n" +
                        "- Avoid free VPNs that may log or sell your data."
                    }
                },

                {
                    "network",
                    new List<string>
                    {
                        "Home network security:\n" +
                        "- Segregate IoT devices on a guest network.\n" +
                        "- Use strong router admin credentials and disable remote management.\n" +
                        "- Monitor connected devices and update firmware regularly."
                    }
                },

                {
                    "social",
                    new List<string>
                    {
                        "Social engineering protection:\n" +
                        "- Be skeptical of unsolicited requests for information (phone, email, social media).\n" +
                        "- Verify identity via a separate channel before sharing sensitive details.\n" +
                        "- Limit what you share publicly on social profiles; attackers use this for impersonation."
                    }
                },

                {
                    "social engineering",
                    new List<string>
                    {
                        "Social engineering protection:\n" +
                        "- Be skeptical of unsolicited requests for information (phone, email, social media).\n" +
                        "- Verify identity via a separate channel before sharing sensitive details.\n" +
                        "- Limit what you share publicly on social profiles; attackers use this for impersonation."
                    }
                },

                {
                    "backup",
                    new List<string>
                    {
                        "Backup best practices:\n" +
                        "1) Follow the 3-2-1 rule: 3 copies, on 2 different media, 1 off-site.\n" +
                        "2) Test restores regularly.\n" +
                        "3) Use encrypted backups for sensitive data.\n" +
                        "4) Automate backups when possible."
                    }
                },

                {
                    "update",
                    new List<string>
                    {
                        "Software update advice:\n" +
                        "- Install OS and application updates promptly to address security vulnerabilities.\n" +
                        "- Enable automatic updates where practical, and keep firmware (router, IoT) up to date."
                    }
                },

                {
                    "2fa",
                    new List<string>
                    {
                        "Multi-factor authentication (MFA) tips:\n" +
                        "- Enable MFA on all accounts that support it, using an authenticator app or hardware key when possible.\n" +
                        "- SMS is better than nothing but is vulnerable to SIM-swapping; prefer app/hardware tokens."
                    }
                },

                {
                    "mfa",
                    new List<string>
                    {
                        "Multi-factor authentication (MFA) tips:\n" +
                        "- Enable MFA on all accounts that support it, using an authenticator app or hardware key when possible.\n" +
                        "- SMS is better than nothing but is vulnerable to SIM-swapping; prefer app/hardware tokens."
                    }
                },

                {
                    "encryption",
                    new List<string>
                    {
                        "Encryption basics:\n" +
                        "- Use device encryption (BitLocker/FileVault) on laptops and phones to protect data if the device is lost or stolen.\n" +
                        "- Use HTTPS websites and consider end-to-end encrypted messaging for sensitive conversations."
                    }
                },

                {
                    "ransomware",
                    new List<string>
                    {
                        "Ransomware prevention & response:\n" +
                        "- Keep backups isolated and up to date (so you can restore rather than pay).\n" +
                        "- Patch systems, avoid risky downloads, and use endpoint protection.\n" +
                        "- If infected, isolate affected machines and seek professional help; avoid paying ransom when possible."
                    }
                },

                {
                    "what is malware",
                    new List<string>
                    {
                        "Malware definition:\n" +
                        "Malware (malicious software) is any software designed to harm, exploit or otherwise compromise a computer system, network or device.\n" +
                        "Common types include viruses, worms, trojans, ransomware, spyware and adware.\n" +
                        "Indicators: unexpected pop-ups, slow performance, files encrypted, unknown processes.\n" +
                        "Prevention: keep software updated, use antivirus, avoid suspicious downloads, and maintain backups."
                    }
                },

                {
                    "define malware",
                    new List<string>
                    {
                        "Malware (malicious software) is code created to infiltrate, damage, or gain unauthorized access to systems. Examples: viruses, worms, trojans, ransomware, spyware."
                    }
                },

                {
                    "what is phishing",
                    new List<string>
                    {
                        "Phishing definition:\n" +
                        "Phishing is a social engineering attack where attackers trick users into revealing sensitive information or performing actions (like clicking malicious links) by impersonating a trusted entity.\n" +
                        "Mitigation: verify senders, don't click unexpected links, enable MFA, and report suspicious messages."
                    }
                },

                {
                    "what is ransomware",
                    new List<string>
                    {
                        "Ransomware definition:\n" +
                        "Ransomware is malware that encrypts a victim's files and demands payment (a ransom) for the decryption key.\n" +
                        "Response: isolate infected devices, do not pay without guidance, restore from backups, and contact incident response professionals."
                    }
                },

                {
                    "attacker",
                    new List<string>
                    {
                        "Attacker / Threat actor overview:\n" +
                        "A threat actor (attacker) is an individual, group or organization that conducts malicious activity against targets.\n" +
                        "Types include: script kiddies, cybercriminals, insiders, hacktivists, and state-sponsored actors (APTs).\n" +
                        "Motivations vary: financial gain, espionage, political goals, or disruption."
                    }
                },

                {
                    "types of attackers",
                    new List<string>
                    {
                        "Common attacker types:\n" +
                        "- Script kiddie: inexperienced attacker using existing tools. Usually opportunistic.\n" +
                        "- Cybercriminal: financially motivated (fraud, ransomware).\n" +
                        "- Insider: a current or former employee with access who misuses it.\n" +
                        "- Hacktivist: politically or socially motivated, aims for publicity.\n" +
                        "- State-sponsored / APT: well-resourced actors conducting long-term espionage or sabotage.\n" +
                        "Mitigations depend on type but include strong access controls, monitoring, and incident response."
                    }
                },

                {
                    "script kiddie",
                    new List<string>
                    {
                        "Script kiddie: an inexperienced attacker who uses pre-packaged tools and scripts to exploit known vulnerabilities. They are less sophisticated but can still cause damage."
                    }
                },

                {
                    "cybercriminal",
                    new List<string>
                    {
                        "Cybercriminal: attackers motivated by financial gain. They run scams, deploy ransomware, steal credit card data, or sell stolen credentials.\n" +
                        "Mitigation: strong financial controls, monitoring, and user education."
                    }
                },

                {
                    "insider",
                    new List<string>
                    {
                        "Insider threat: a trusted person (employee, contractor) who intentionally or unintentionally causes harm.\n" +
                        "Mitigation: least privilege, logging, user behavior analytics, and clear policies."
                    }
                },

                {
                    "hacktivist",
                    new List<string>
                    {
                        "Hacktivist: attackers motivated by political or social causes. They often deface websites or leak data to embarrass targets.\n" +
                        "Mitigation: good perimeter security, regular backups, and public communication plans."
                    }
                },

                {
                    "apt",
                    new List<string>
                    {
                        "APT (Advanced Persistent Threat): a prolonged and targeted cyberattack, often state-sponsored, with stealthy, sophisticated techniques aimed at espionage or long-term access.\n" +
                        "Mitigation: layered defenses, threat intelligence, endpoint detection and response (EDR), and professional incident response."
                    }
                },

                {
                    "privacy",
                    new List<string>
                    {
                        "Review your privacy settings regularly.",
                        "Avoid sharing personal details publicly.",
                        "Use two-factor authentication when possible."
                    }
                }
            };
        }

        public string GetResponse(string input)
        {
            string matchedKey;
            return GetResponseWithKey(input, out matchedKey);
        }

        // Return all known keywords (useful for "what can I ask" responses)
        public List<string> GetAllKeywords()
        {
            return new List<string>(_responses.Keys);
        }

        // Returns response and outputs the matched dictionary key (if any)
        public string GetResponseWithKey(string input, out string matchedKey)
        {
            matchedKey = null;

            if (string.IsNullOrWhiteSpace(input))
                return null;

            input = input.ToLower();

            // naive normalization: remove punctuation and extra spaces
            var normalized = new string(input.Where(c => !char.IsPunctuation(c)).ToArray());

            foreach (var kvp in _responses)
            {
                var keyword = kvp.Key.ToLower();

                // direct substring match
                if (normalized.Contains(keyword))
                {
                    List<string> replies = kvp.Value;
                    matchedKey = kvp.Key; // return original key casing
                    return replies[_random.Next(replies.Count)];
                }

                // allow simple plural/singular variation matching (passwords -> password)
                if (keyword.EndsWith("s") && normalized.Contains(keyword.TrimEnd('s')))
                {
                    List<string> replies = kvp.Value;
                    matchedKey = kvp.Key;
                    return replies[_random.Next(replies.Count)];
                }

                if (!keyword.EndsWith("s") && normalized.Contains(keyword + "s"))
                {
                    List<string> replies = kvp.Value;
                    matchedKey = kvp.Key;
                    return replies[_random.Next(replies.Count)];
                }
            }

            return null;
        }
    }
}