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
        contacts.Add(new Contact("jiwoo", "01092139670"));
        contacts.Add(new Contact("sumin", "01020182373"));
        contacts.Add(new Contact("ksi", "01092289010"));
        contacts.Add(new Contact("mkr", "01064772374"));
        contacts.Add(new Contact("kjw", "01088309670"));

        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });

        searchButton.onClick.AddListener(SearchContact);
        addButton.onClick.AddListener(AddContact);
        deleteButton.onClick.AddListener(DeleteContact);

        DropdownValueChanged(dropdown);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DropdownValueChanged(Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 0:
                searchSession.SetActive(true);
                addSession.SetActive(false);
                deleteSession.SetActive(false);
                break;
            case 1:
                searchSession.SetActive(false);
                addSession.SetActive(true);
                deleteSession.SetActive(false);
                break;
            case 2:
                searchSession.SetActive(false);
                addSession.SetActive(false);
                deleteSession.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void SearchContact()
    {
        string searchQuery = searchInput.text.Trim();
        searchResultText.text = "";

        IEnumerable<Contact> results;

        // 쿼리가 비어있다면 전체 출력
        if (searchQuery == "")
        {
             results = contacts;
        }
        else
        {
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