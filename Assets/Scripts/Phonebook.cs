using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Phonebook : MonoBehaviour
{
    // Data
    private List<Contact> contacts = new List<Contact>();

    // UI Bind
    [SerializeField] public Dropdown dropdown;
    [SerializeField] public GameObject searchSession;
    [SerializeField] public InputField searchInput;
    [SerializeField] public Button searchButton;
    [SerializeField] public Text searchResultText;
    [SerializeField] public GameObject addSession;
    [SerializeField] public InputField addInputName;
    [SerializeField] public InputField addInputPhone;
    [SerializeField] public Button addButton;
    [SerializeField] public Text addResultText;
    [SerializeField] public GameObject deleteSession;
    [SerializeField] public InputField deleteInput;
    [SerializeField] public Button deleteButton;
    [SerializeField] public Text deleteResultText;

    

    // Start is called before the first frame update
    void Start()
    {
        // Add Data
        contacts.Add(new Contact("kangjiwoo", "01092139670"));
        contacts.Add(new Contact("kangsumin", "01020182374"));
        contacts.Add(new Contact("kangjaewook", "01085129670"));
        contacts.Add(new Contact("kangsungil", "01088329228"));
        contacts.Add(new Contact("moonkyungran", "01064772374"));

        // Add Listen to UI
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });

        searchButton.onClick.AddListener(SearchContact);
        addButton.onClick.AddListener(AddContact);
        deleteButton.onClick.AddListener(DeleteContact);

        // Init Setting
        DropdownValueChanged(dropdown);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DropdownValueChanged(Dropdown dropdown)
    {
        searchSession.SetActive(dropdown.value == 0);
        addSession.SetActive(dropdown.value == 1);
        deleteSession.SetActive(dropdown.value == 2);
    }

    public void SearchContact()
    {
        string searchQuery = searchInput.text.Trim().ToLower();
        // searchResultText.text = "";

        // 검색어가 비었다면 전체 출력
        if (string.IsNullOrEmpty(searchQuery))
        {
            searchResultText.text = string.Join("\n", contacts.Select(contact => $"Name: {contact.Name}, Phone: {contact.PhoneNumber}"));

            return;
        }

        Regex numRegex = new Regex(@"^[0-9]+$");
        Regex alphaRegex = new Regex(@"^[a-zA-Z]+$");

        List<Contact> results = new List<Contact>();

        // 검색어에 영어만 쓰면 -> 이름으로 서치
        // 검색어에 숫자만 쓰면 -> 전화번호로 서치
        // 검색어에 섞어쓰면 -> 예외 처리
        if (alphaRegex.IsMatch(searchQuery))
        {
            results = BinarySearchOnName(searchQuery);
        }
        else if (numRegex.IsMatch(searchQuery))
        {
            results = BinarySearchOnNumber(searchQuery);
        }
        else
        {
            searchResultText.text = "Input only Numbers or only English";
            return;
        }

        // 결과가 비어있다면 -> 예외 처리
        if (results.Any())
        {
            searchResultText.text = string.Join("\n", results.Select(contact => $"Name: {contact.Name}, Phone: {contact.PhoneNumber}"));
        }
        else
        {
            searchResultText.text = "No contacts found";
        }

        /*
        IEnumerable<Contact> results;

        // 쿼리가 비어있다면 전체 출력
        if (searchQuery == "")
        {
            results = contacts;
        }
        else
        {
            BinarySearchContact(searchQuery);

            results = contacts.Where(contact => contact.Name.Contains(searchQuery) || contact.PhoneNumber.Contains(searchQuery));
        }

        // 서치 결과가 없다면 예외 처리
        if (results.Any())
        {
            results = results.OrderBy(contact => contact.Name);

            foreach (var contact in results)
            {
                searchResultText.text += $"Name: {contact.Name}, Phone: {contact.PhoneNumber}\n";
            }
        }
        else
        {
            searchResultText.text = "No contacts found";
        }
        */
    }

    private List<Contact> BinarySearchOnName(string searchQuery)
    {
        int left = 0;
        int right = contacts.Count - 1;

        // 오름차순 정렬
        contacts = contacts.OrderBy(contact => contact.Name).ToList();

        List<Contact> results = new List<Contact>();

        while (left <= right)
        {
            int mid = (left + right) / 2;
            var contact = contacts[mid];

            // 검색어가 포함되면
            if (contact.Name.ToLower().Contains(searchQuery))
            {
                results.Add(contact);

                // 범위를 왼쪽으로 확장
                int temp = mid - 1;
                while (temp >= 0 && contacts[temp].Name.ToLower().Contains(searchQuery))
                {
                    results.Add(contacts[temp]);
                    temp -= 1;
                }

                // 범위를 오른쪽으로 확장
                temp = mid + 1;
                while (temp < contacts.Count && contacts[temp].Name.ToLower().Contains(searchQuery))
                {
                    results.Add(contacts[temp]);
                    temp += 1;
                }

                break;
            }

            if (string.Compare(contact.Name, searchQuery, true) < 0)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return results.OrderBy(contact => contact.Name).ToList();
    }

    private List<Contact> BinarySearchOnNumber(string searchQuery)
    {
        int left = 0;
        int right = contacts.Count - 1;

        // 오름차순 정렬
        contacts = contacts.OrderBy(contact => contact.PhoneNumber).ToList();

        List<Contact> results = new List<Contact>();

        while (left <= right)
        {
            int mid = (left + right) / 2;
            var contact = contacts[mid];

            // 검색어가 포함되면
            if (contact.PhoneNumber.Contains(searchQuery))
            {
                results.Add(contact);

                // 범위를 왼쪽으로 확장
                int temp = mid - 1;
                while (temp >= 0 && contacts[temp].PhoneNumber.Contains(searchQuery))
                {
                    results.Add(contacts[temp]);
                    temp -= 1;
                }

                // 범위를 오른쪽으로 확장
                temp = mid + 1;
                while (temp < contacts.Count && contacts[temp].PhoneNumber.Contains(searchQuery))
                {
                    results.Add(contacts[temp]);
                    temp += 1;
                }

                break;
            }

            if (string.Compare(contact.PhoneNumber, searchQuery, true) < 0)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return results.OrderBy(contact => contact.PhoneNumber).ToList();
    }

    public void AddContact()
    {
        string nameQuery = addInputName.text.Trim();
        string phoneQuery = addInputPhone.text.Trim();

        // 쿼리에 값이 들어있지 않으면 리턴
        if (nameQuery == "" || phoneQuery == "" )
        {
            addResultText.text = "Enter Values";
            return;
        }

        // 전화번호가 숫자가 아니면 리턴
        Regex regex = new Regex(@"^[0-9]+$");
        if (!regex.IsMatch(phoneQuery))
        {
            addResultText.text = "Phone number must contain only numbers";
            return;
        }

        // 리스트에 이미 값이 존재하면 리턴
        if (contacts.Exists(contact => contact.Name == nameQuery) || contacts.Exists(contact => contact.PhoneNumber == phoneQuery))
        {
            addResultText.text = "Already Exist";
            return;
        }

        Contact newContact = new Contact(nameQuery, phoneQuery);
        contacts.Add(newContact);

        addResultText.text = "Success to add\n";
        addResultText.text = $"Name: {newContact.Name}, Phone: {newContact.PhoneNumber}";
    }

    public void DeleteContact()
    {
        string searchQuery = deleteInput.text.Trim();

        Contact result = contacts.Find(contact => 
                contact.Name.Contains(searchQuery));

        if (result != null)
        {
            contacts.Remove(result);
            deleteResultText.text = $"Success to delete {result.Name}";
        }
        else
        {
            deleteResultText.text = "Not found";
        }
    }
}

public class Contact
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }

    public Contact(string name, string phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;
    }
}
